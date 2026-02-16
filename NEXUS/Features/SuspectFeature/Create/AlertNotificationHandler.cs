using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Services.Notification;

namespace NEXUS.Features.SuspectFeature.Create;

public sealed class AlertNotificationHandler(AppDbContext db, INotificationService notificationService)
{
    public async Task Handle(Events.SecurityAlertRaised notification)
    {
        var alert = new SystemAlert
        {
            Id = Guid.NewGuid(),
            Title = "Security Alert: High Risk Suspect",
            Message = $"{notification.Message}\nRecommendation: {notification.Recommendation}",
            Level = notification.Level,
            Source = notification.Source,
            RelatedEntityId = notification.SuspectId,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        db.SystemAlerts.Add(alert);
        await db.SaveChangesAsync();

        // Push to Command Center / Clients
        await notificationService.SendAlertToCommandersAsync(alert);
    }
}
