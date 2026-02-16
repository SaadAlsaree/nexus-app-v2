using Microsoft.AspNetCore.SignalR;
using NEXUS.Data.Entities;
using NEXUS.Infrastructure.Hubs;

namespace NEXUS.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub, IAlertClient> _hubContext;

    public NotificationService(IHubContext<NotificationHub, IAlertClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendAlertToAllAsync(SystemAlert alert)
    {
        // إرسال التنبيه للجميع
        await _hubContext.Clients.All.ReceiveSecurityAlert(alert);
    }

    public async Task SendAlertToCommandersAsync(SystemAlert alert)
    {
        // إرسال لمجموعة القادة فقط
        await _hubContext.Clients.Group("Commanders").ReceiveSecurityAlert(alert);
    }

    public async Task SendGraphRefreshSignalAsync(string suspectId)
    {
        // إرسال أمر تحديث الغراف لكل المحققين
        // ليروا العلاقة الجديدة التي ظهرت للتو
        await _hubContext.Clients.All.RefreshGraphView(suspectId);
    }

    public async Task SendUserNotificationAsync(string userId, string message)
    {
        // إرسال لمستخدم محدد
        await _hubContext.Clients.Group($"User_{userId}")
            .NotifyOperationComplete(message, true);
    }
}
