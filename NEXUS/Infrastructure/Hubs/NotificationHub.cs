using Microsoft.AspNetCore.SignalR;

namespace NEXUS.Infrastructure.Hubs;

public class NotificationHub : Hub<IAlertClient>
{
    // يتم استدعاؤها تلقائياً عند اتصال المستخدم (F5 or Login)
    public override async Task OnConnectedAsync()
    {
        // 1. إضافة المستخدم لمجموعته الخاصة (لإرسال رسائل خاصة له)
        string userId = Context.UserIdentifier!;
        await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");

        // 2. إضافة المستخدم لمجموعة حسب دوره (Role)
        // (مثلاً: تنبيهات القادة لا تصل للمحققين العاديين)
        if (Context.User!.IsInRole("Admin") || Context.User.IsInRole("Commander"))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Commanders");
        }
        else
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Investigators");
        }

        // 3. إضافة المستخدم لمجموعة "القسم" الخاص به
        // var department = Context.User.FindFirst("Department")?.Value;
        // if (department != null) await Groups.AddToGroupAsync(Context.ConnectionId, $"Dept_{department}");

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // تنظيف المجموعات يتم تلقائياً بواسطة SignalR، لكن يمكن إضافة منطق هنا
        // مثل تسجيل خروج المستخدم في سجل الرقابة
        await base.OnDisconnectedAsync(exception);
    }
}

