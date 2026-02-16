using NEXUS.Data.Entities;

namespace NEXUS.Infrastructure.Hubs;

public interface IAlertClient
{
    // 1. استقبال تنبيه أمني جديد (نافذة منبثقة أو جرس)
    Task ReceiveSecurityAlert(SystemAlert alert);

    // 2. تحديث عداد التنبيهات غير المقروءة في الشريط العلوي
    Task UpdateUnreadCount(int count);

    // 3. أمر للمتصفح بتحديث رسم الشبكة (Graph) لوجود بيانات جديدة
    // (مفيد جداً للمحللين الذين يراقبون شاشة الرسم البياني)
    Task RefreshGraphView(string suspectId);

    // 4. إشعار بأن عملية استيراد البيانات الطويلة انتهت
    Task NotifyOperationComplete(string message, bool isSuccess);
}