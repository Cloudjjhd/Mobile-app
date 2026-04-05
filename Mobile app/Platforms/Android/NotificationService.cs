using global::Android.App;
using global::Android.Content;
using global::Android.OS;
using AndroidX.Core.App;
using Mobile_app.Models;
using Mobile_app.Services;

namespace Mobile_app.Platforms.Android;

public class NotificationService : INotificationService
{
    private const string ChannelId = "focusday_reminders";

    public Task RequestPermissionAsync()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
        {
            var activity = Platform.CurrentActivity;
            if (activity != null)
            {
                activity.RequestPermissions(
                    new[] { global::Android.Manifest.Permission.PostNotifications }, 1001);
            }
        }

        CreateNotificationChannel();
        return Task.CompletedTask;
    }

    public Task ScheduleNotificationAsync(NotificationData notification)
    {
        var context = Platform.AppContext;

        Intent intent = new Intent(context, typeof(NotificationReceiver));
        intent.PutExtra("title", notification.Title);
        intent.PutExtra("message", notification.Message);
        intent.PutExtra("id", notification.Id);

        PendingIntent pendingIntent = PendingIntent.GetBroadcast(
            context,
            notification.Id,
            intent,
            PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

        var alarmManager = (AlarmManager?)context.GetSystemService(Context.AlarmService);

        long triggerTime = new DateTimeOffset(notification.NotifyTime).ToUnixTimeMilliseconds();

        alarmManager?.SetAndAllowWhileIdle(
            AlarmType.RtcWakeup,
            triggerTime,
            pendingIntent);

        return Task.CompletedTask;
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(
                ChannelId,
                "FocusDay Reminders",
                NotificationImportance.High)
            {
                Description = "Study reminder notifications"
            };

            var manager = (NotificationManager?)Platform.AppContext
                .GetSystemService(Context.NotificationService);

            manager?.CreateNotificationChannel(channel);
        }
    }

    public static void ShowNotification(Context context, string title, string message, int id)
    {
        var builder = new NotificationCompat.Builder(context, ChannelId)
            .SetContentTitle(title)
            .SetContentText(message)
            .SetSmallIcon(Resource.Mipmap.appicon)
            .SetPriority((int)NotificationPriority.High)
            .SetAutoCancel(true);

        NotificationManagerCompat.From(context).Notify(id, builder.Build());
    }
}