using global::Android.Content;

namespace Mobile_app.Platforms.Android;

[BroadcastReceiver(Enabled = true, Exported = true)]
public class NotificationReceiver : BroadcastReceiver
{
    public override void OnReceive(Context? context, Intent? intent)
    {
        if (context == null || intent == null)
            return;

        string title = intent.GetStringExtra("title") ?? "FocusDay Reminder";
        string message = intent.GetStringExtra("message") ?? "It's time to study.";
        int id = intent.GetIntExtra("id", 0);

        global::Mobile_app.Platforms.Android.NotificationService.ShowNotification(context, title, message, id);
    }
}