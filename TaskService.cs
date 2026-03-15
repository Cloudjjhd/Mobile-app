using Mobile_app.Models;

namespace Mobile_app.Services;

public static class TaskService
{
    public static List<StudyTask> Tasks { get; set; } = new()
    {
        new StudyTask
        {
            Subject = "Math Revision",
            Date = new DateTime(2025, 8, 12),
            TimeText = "09:00",
            Category = "Revision",
            Duration = "1 hour",
            Notes = "Complete Chapter 3 exercises",
            IsCompleted = false
        },
        new StudyTask
        {
            Subject = "English Essay",
            Date = new DateTime(2025, 8, 12),
            TimeText = "11:00",
            Category = "Assignment",
            Duration = "45 minutes",
            Notes = "Work on introduction and body paragraph",
            IsCompleted = true
        },
        new StudyTask
        {
            Subject = "Physics Review",
            Date = new DateTime(2025, 8, 13),
            TimeText = "16:00",
            Category = "Revision",
            Duration = "45 minutes",
            Notes = "Focus on Chapter 5 formulas",
            IsCompleted = false
        }
    };
}
