using Mobile_app.Models;
using Mobile_app.Services;

namespace Mobile_app.Pages;

public partial class TaskDetailsPage : ContentPage
{
    private readonly StudyTask task;

    public TaskDetailsPage(StudyTask selectedTask)
    {
        InitializeComponent();
        task = selectedTask;
        LoadTask();
    }

    private void LoadTask()
    {
        SubjectLabel.Text = $"Subject: {task.Subject}";
        DateLabel.Text = $"Date: {task.Date:dddd, dd MMMM yyyy}";
        TimeLabel.Text = $"Time: {task.TimeText}";
        CategoryLabel.Text = $"Category: {task.Category}";
        DurationLabel.Text = $"Duration: {task.Duration}";
        NotesLabel.Text = task.Notes;
        StatusLabel.Text = $"Status: {(task.IsCompleted ? "Completed" : "Not Completed")}";

        StatusLabel.TextColor = task.IsCompleted ? Colors.Green : Colors.OrangeRed;
    }

    private async void OnMarkAsDoneClicked(object sender, EventArgs e)
    {
        task.IsCompleted = true;
        await TaskService.SaveTasksAsync();
        LoadTask();

        await DisplayAlertAsync("Updated", "Task marked as completed.", "OK");
        await Navigation.PopAsync();
    }

    private async void OnEditTaskClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddStudyTaskPage(task));
    }

    private async void OnDeleteTaskClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlertAsync("Confirm", "Do you want to delete this task?", "Yes", "No");

        if (confirm)
        {
            TaskService.Tasks.Remove(task);
            await TaskService.SaveTasksAsync();
            await Navigation.PopAsync();
        }
    }
}