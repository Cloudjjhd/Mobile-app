using System.Text.Json;
using Mobile_app.Models;

namespace Mobile_app.Services;

public static class TaskService
{
    private static readonly string filePath =
        Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

    public static List<StudyTask> Tasks { get; set; } = new();

    public static async Task InitializeAsync()
    {
        if (File.Exists(filePath))
        {
            string json = await File.ReadAllTextAsync(filePath);

            if (!string.IsNullOrWhiteSpace(json))
            {
                var loadedTasks = JsonSerializer.Deserialize<List<StudyTask>>(json);
                Tasks = loadedTasks ?? new List<StudyTask>();
            }
        }

        if (Tasks.Count == 0)
        {
            Tasks = new List<StudyTask>
            {
                new StudyTask
                {
                    Subject = "Math Revision",
                    Date = DateTime.Today,
                    TimeText = "09:00",
                    Category = "Revision",
                    Duration = "1 hour",
                    Notes = "Complete Chapter 3 exercises",
                    IsCompleted = false
                },
                new StudyTask
                {
                    Subject = "English Essay",
                    Date = DateTime.Today,
                    TimeText = "11:00",
                    Category = "Assignment",
                    Duration = "45 minutes",
                    Notes = "Work on introduction and body paragraph",
                    IsCompleted = true
                },
                new StudyTask
                {
                    Subject = "Physics Review",
                    Date = DateTime.Today.AddDays(1),
                    TimeText = "16:00",
                    Category = "Revision",
                    Duration = "45 minutes",
                    Notes = "Focus on Chapter 5 formulas",
                    IsCompleted = false
                }
            };

            await SaveTasksAsync();
        }
    }

    public static async Task SaveTasksAsync()
    {
        var json = JsonSerializer.Serialize(Tasks, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(filePath, json);
    }
}