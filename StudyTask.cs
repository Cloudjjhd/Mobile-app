namespace Mobile_app.Models;

public class StudyTask
{
    public string Subject { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Today;
    public string TimeText { get; set; } = "";
    public string Category { get; set; } = "";
    public string Duration { get; set; } = "";
    public string Notes { get; set; } = "";
    public bool IsCompleted { get; set; } = false;
}