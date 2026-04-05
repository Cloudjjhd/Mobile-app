using Mobile_app.Models;
using Mobile_app.Services;

namespace Mobile_app.Pages;

public partial class AddStudyTaskPage : ContentPage
{
    private StudyTask? editingTask;
    private readonly INotificationService? notificationService;

    public AddStudyTaskPage()
    {
        InitializeComponent();

        notificationService = Application.Current?.Handler?.MauiContext?.Services
            .GetService<INotificationService>();

        TaskDatePicker.Date = DateTime.Today;
        ReminderPicker.SelectedIndex = 0; // 默认 1 分钟
    }

    public AddStudyTaskPage(StudyTask taskToEdit)
    {
        InitializeComponent();

        notificationService = Application.Current?.Handler?.MauiContext?.Services
            .GetService<INotificationService>();

        editingTask = taskToEdit;

        SubjectEntry.Text = taskToEdit.Subject;
        TaskDatePicker.Date = taskToEdit.Date;
        TimeEntry.Text = taskToEdit.TimeText;
        CategoryEntry.Text = taskToEdit.Category;
        DurationEntry.Text = taskToEdit.Duration;
        NotesEditor.Text = taskToEdit.Notes;

        ReminderSwitch.IsToggled = taskToEdit.HasReminder;

        var reminderValues = new List<string> { "1", "5", "10", "30" };
        int index = reminderValues.IndexOf(taskToEdit.ReminderMinutesBefore.ToString());
        ReminderPicker.SelectedIndex = index >= 0 ? index : 0;
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

        bool hasReminder = ReminderSwitch.IsToggled;
        int reminderMinutes = int.Parse(ReminderPicker.SelectedItem?.ToString() ?? "1");

        StudyTask targetTask;

        if (editingTask == null)
        {
            targetTask = new StudyTask
            {
                Subject = SubjectEntry.Text ?? "",
                Date = TaskDatePicker.Date ?? DateTime.Today,
                TimeText = TimeEntry.Text ?? "",
                Category = CategoryEntry.Text ?? "",
                Duration = DurationEntry.Text ?? "",
                Notes = NotesEditor.Text ?? "",
                IsCompleted = false,
                HasReminder = hasReminder,
                ReminderMinutesBefore = reminderMinutes
            };

            TaskService.Tasks.Add(targetTask);
        }
        else
        {
            editingTask.Subject = SubjectEntry.Text ?? "";
            editingTask.Date = TaskDatePicker.Date ?? DateTime.Today;
            editingTask.TimeText = TimeEntry.Text ?? "";
            editingTask.Category = CategoryEntry.Text ?? "";
            editingTask.Duration = DurationEntry.Text ?? "";
            editingTask.Notes = NotesEditor.Text ?? "";
            editingTask.HasReminder = hasReminder;
            editingTask.ReminderMinutesBefore = reminderMinutes;

            targetTask = editingTask;
        }

        await TaskService.SaveTasksAsync();

        try
        {
            await ScheduleReminderIfNeeded(targetTask);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Debug", $"ScheduleReminderIfNeeded crashed: {ex.Message}", "OK");
        }

        if (editingTask == null)
            await DisplayAlertAsync("Success", "Study task added successfully.", "OK");
        else
            await DisplayAlertAsync("Updated", "Study task updated successfully.", "OK");

        await Navigation.PopAsync();
    }

    private async Task ScheduleReminderIfNeeded(StudyTask task)
    {
        if (!task.HasReminder)
        {
            await DisplayAlertAsync("Debug", "Reminder is OFF", "OK");
            return;
        }

        if (notificationService == null)
        {
            await DisplayAlertAsync("Debug", "notificationService is null", "OK");
            return;
        }

        string rawDateTime = $"{task.Date:yyyy-MM-dd} {task.TimeText}";

        if (!DateTime.TryParse(rawDateTime, out DateTime taskDateTime))
        {
            await DisplayAlertAsync("Debug", $"Time parse failed: {rawDateTime}", "OK");
            return;
        }

        DateTime notifyTime = taskDateTime.AddMinutes(-task.ReminderMinutesBefore);

        await DisplayAlertAsync(
            "Debug",
            $"Task time: {taskDateTime}\nNotify time: {notifyTime}\nNow: {DateTime.Now}",
            "OK");

        if (notifyTime <= DateTime.Now)
        {
            await DisplayAlertAsync("Debug", "Notify time is already in the past", "OK");
            return;
        }

        try
        {
            await notificationService.RequestPermissionAsync();
            await DisplayAlertAsync("Debug", "Permission request finished", "OK");

            await notificationService.ScheduleNotificationAsync(new NotificationData
            {
                Id = Random.Shared.Next(1000, 999999),
                Title = "FocusDay Reminder",
                Message = $"{task.Subject} starts in {task.ReminderMinutesBefore} minute(s).",
                NotifyTime = notifyTime
            });

            await DisplayAlertAsync("Debug", "Reminder scheduled successfully", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Debug", $"Notification error: {ex.Message}", "OK");
        }
    }
}