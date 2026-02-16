using NEXUS.Data.Entities;

namespace NEXUS.Services.Notification;

public interface INotificationService
{
    Task SendAlertToAllAsync(SystemAlert alert);
    Task SendAlertToCommandersAsync(SystemAlert alert); // للخطورة العالية
    Task SendGraphRefreshSignalAsync(string suspectId);
    Task SendUserNotificationAsync(string userId, string message);
}