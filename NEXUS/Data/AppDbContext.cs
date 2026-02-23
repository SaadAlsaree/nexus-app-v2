using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NEXUS.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<BayahDetail> BayahDetails { get; set; }
    public DbSet<Case> Cases { get; set; }
    public DbSet<CaseSuspect> CaseSuspects { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<EntityWatchlist> EntityWatchlists { get; set; }
    public DbSet<EvidenceAttachment> EvidenceAttachments { get; set; }
    public DbSet<LifeHistory> LifeHistories { get; set; }
    public DbSet<Operation> Operations { get; set; }
    public DbSet<OrgUnit> OrgUnits { get; set; }
    public DbSet<RelatedPerson> RelatedPeople { get; set; }
    public DbSet<Suspect> Suspects { get; set; }
    public DbSet<SystemAuditLog> SystemAuditLogs { get; set; }
    public DbSet<TrainingCourse> TrainingCourses { get; set; }
    public DbSet<SystemAlert> SystemAlerts { get; set; }
    public DbSet<InterrogationSession> InterrogationSessions { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
    public DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
    public DbSet<IdentityUserLogin<string>> UserLogins { get; set; }
    public DbSet<IdentityRoleClaim<string>> RoleClaims { get; set; }
    public DbSet<IdentityUserToken<string>> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
