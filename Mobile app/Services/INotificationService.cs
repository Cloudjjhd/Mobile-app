using Mobile_app.Models;

namespace Mobile_app.Services;

public interface INotificationService
{
    Task RequestPermissionAsync();
    Task ScheduleNotificationAsync(NotificationData notification);
}