using NEXUS.BackgroundJobs;
using NEXUS.Data;
using NEXUS.Data.Entities;
using NEXUS.Data.Enums;
using NEXUS.Services.Notification;

namespace NEXUS.Features.AlertNotification;

public class AlertNotificationHandler
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notifier;
    // private readonly IHubContext<AlertHub> _hub; // لـ SignalR

    public AlertNotificationHandler(AppDbContext db, INotificationService notifier)
    {
        _db = db;
        _notifier = notifier;

    }

    public async Task Handle(SecurityAlertRaised alert)
    {
        // 1. تحويل الحدث لكيان قاعدة بيانات
        var systemAlert = new SystemAlert
        {
            Id = Guid.NewGuid(),
            Title = alert.Title,
            Message = alert.Message,
            Level = alert.Level,
            Source = alert.Source,
            RelatedEntityId = alert.RelatedEntityId,
            CreatedAt = DateTime.UtcNow
        };

        // 2. الحفظ في SQL
        _db.SystemAlerts.Add(systemAlert);
        await _db.SaveChangesAsync();

        // 3. الإرسال الفوري (Real-time) حسب الخطورة
        if (alert.Level == AlertLevel.RedAlert || alert.Level == AlertLevel.High)
        {
            // التنبيهات الخطرة جداً تذهب للقادة فوراً
            await _notifier.SendAlertToCommandersAsync(systemAlert);
        }
        else
        {
            // التنبيهات العادية تذهب للجميع (أو للمحققين فقط)
            await _notifier.SendAlertToAllAsync(systemAlert);
        }
    }
}