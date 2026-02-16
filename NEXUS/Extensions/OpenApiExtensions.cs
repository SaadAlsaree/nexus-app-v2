using Scalar.AspNetCore;

namespace NEXUS.Extensions;

public static class OpenApiExtensions
{

    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection("OpenApi");

        if (!openApiSection.Exists())
        {
            return app;
        }

        // Map OpenAPI endpoint
        app.MapOpenApi();

        // Configure Scalar UI in development mode
        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference(options =>
            {
                // Disable default fonts to avoid downloading unnecessary fonts
                options.DefaultFonts = false;

                // Set custom title from configuration
                options.Title = openApiSection["Document:Title"] ?? "API Documentation";
            });

            // Redirect root to Scalar UI
            app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
        }

        return app;
    }


    public static IHostApplicationBuilder AddDefaultOpenApi(this IHostApplicationBuilder builder)
    {
        var openApi = builder.Configuration.GetSection("OpenApi");

        if (!openApi.Exists())
        {
            return builder;
        }

        // Add OpenAPI with custom document info
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Title = openApi["Document:Title"] ?? "API";
                document.Info.Description = openApi["Document:Description"] ?? "API Documentation";
                document.Info.Version = "v1";
                return Task.CompletedTask;
            });
        });

        return builder;
    }
}