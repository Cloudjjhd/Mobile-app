namespace Mobile_app.Models;

public class NotificationData
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public DateTime NotifyTime { get; set; }
}