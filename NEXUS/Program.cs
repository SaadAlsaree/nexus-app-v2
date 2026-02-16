using Azure.Storage.Blobs;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using NEXUS.Data;
using NEXUS.Extensions;
using NEXUS.Infrastructure.Hubs;
using NEXUS.Services;
using NEXUS.Services.Notification;
using NEXUS.Services.SqlSearch;
using NEXUS.Services.Storage;
using NEXUS.Services.TextAnalysis;
using Wolverine;
using Wolverine.FluentValidation;

var builder = WebApplication.CreateBuilder(args);
//ErrorHandling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Initialize QuestPDF License
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

// Add services to the container.203

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// قراءة القيم باستخدام المفاتيح الصحيحة
var neo4jUri = builder.Configuration["Neo4j:Uri"];
var neo4jUser = builder.Configuration["Neo4j:User"];
var neo4jPass = builder.Configuration["Neo4j:Password"];

var BlobStorageConnectionString = builder.Configuration.GetConnectionString("BlobStorage");

// التحقق من أن القيم ليست فارغة (ممارسة جيدة)
if (string.IsNullOrEmpty(neo4jUri) || string.IsNullOrEmpty(neo4jUser) || string.IsNullOrEmpty(neo4jPass))
{
    throw new InvalidOperationException("Neo4j configuration is missing in appsettings.json");
}

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string is missing in appsettings.json");
}



// 1. Entity Framework
builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Neo4j Driver 
builder.Services.AddSingleton<IDriver>(sp =>
    GraphDatabase.Driver(neo4jUri, AuthTokens.Basic(neo4jUser, neo4jPass)));

// 3. إضافة خدمات Wolverine إلى حاوية الخدمات
builder.Host.UseWolverine(options =>
{
    options.ApplicationAssembly = typeof(Program).Assembly;
    options.ServiceName = "NEXUS";
    options.UseFluentValidation();

    // تفعيل HTTP endpoints لـ Wolverine
    options.Policies.AutoApplyTransactions();

    // Configure HTTP options to prevent concurrent route access issues
    options.Discovery.IncludeAssembly(typeof(Program).Assembly);
});

// 4. Blob Storage
builder.Services.AddSingleton(_ => new BlobServiceClient(BlobStorageConnectionString));


// builder.Services.AddWolverineHttp(); // Removed WolverineHttp
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddHttpContextAccessor();

// 5. mapping
var config = TypeAdapterConfig.GlobalSettings;
// يبحث في الـ Assembly عن أي كلاس ينفذ IRegister ويسجله
config.Scan(System.Reflection.Assembly.GetExecutingAssembly());

// إضافة خدمة الـ Mapper
builder.Services.AddMapster();


// تسجيل الخدمة
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ISqlSearchEngine, SqlSearchEngine>();
builder.Services.AddScoped<IIntelligenceEngine, IntelligenceEngine>();
builder.Services.AddScoped<ITextAnalysisService, TextAnalysisService>();
builder.Services.AddScoped<IBlobService, BlobService>();
builder.Services.AddScoped<IPdfService, NEXUS.Infrastructure.Reports.QuestPdfService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("NEXUS", builder =>
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials());
});

var app = builder.Build();


app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseDefaultOpenApi();
}

app.UseCors("NEXUS");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

// apply migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<AppDbContext>();

        // This will create the database if it does not exist and apply any pending migrations
        context.Database.Migrate();

        logger.LogInformation("Database migrated successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database");
    }
}

app.Run();
