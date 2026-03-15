using Mobile_app.Models;
using Mobile_app.Services;

namespace Mobile_app.Pages;

public partial class AddStudyTaskPage : ContentPage
{
    private StudyTask? editingTask;

    public AddStudyTaskPage()
    {
        InitializeComponent();
        TaskDatePicker.Date = DateTime.Today;
    }

    public AddStudyTaskPage(StudyTask taskToEdit)
    {
        InitializeComponent();

        editingTask = taskToEdit;

        SubjectEntry.Text = taskToEdit.Subject;
        TaskDatePicker.Date = taskToEdit.Date;
        TimeEntry.Text = taskToEdit.TimeText;
        CategoryEntry.Text = taskToEdit.Category;
        DurationEntry.Text = taskToEdit.Duration;
        NotesEditor.Text = taskToEdit.Notes;
    }

    private async void OnSaveTaskClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SubjectEntry.Text))
        {
            await DisplayAlertAsync("Missing Information", "Please enter a subject.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(TimeEntry.Text))
        {
            await DisplayAlertAsync("Missing Information", "Please enter a time.", "OK");
            return;
        }

        if (editingTask == null)
        {
            var task = new StudyTask
            {
                Subject = SubjectEntry.Text ?? "",
                Date = TaskDatePicker.Date ?? DateTime.Today,
                TimeText = TimeEntry.Text ?? "",
                Category = CategoryEntry.Text ?? "",
                Duration = DurationEntry.Text ?? "",
                Notes = NotesEditor.Text ?? "",
                IsCompleted = false
            };

            TaskService.Tasks.Add(task);

            await DisplayAlertAsync("Success", "Study task added successfully.", "OK");
        }
        else
        {
            editingTask.Subject = SubjectEntry.Text ?? "";
            editingTask.Date = TaskDatePicker.Date ?? DateTime.Today;
            editingTask.TimeText = TimeEntry.Text ?? "";
            editingTask.Category = CategoryEntry.Text ?? "";
            editingTask.Duration = DurationEntry.Text ?? "";
            editingTask.Notes = NotesEditor.Text ?? "";

            await DisplayAlertAsync("Updated", "Study task updated successfully.", "OK");
        }

        await Navigation.PopAsync();
    }
}