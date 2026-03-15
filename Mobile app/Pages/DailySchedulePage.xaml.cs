using Mobile_app.Models;
using Mobile_app.Services;

namespace Mobile_app.Pages;

public partial class DailySchedulePage : ContentPage
{
    private DateTime selectedDate = DateTime.Today;

    public DailySchedulePage()
    {
        InitializeComponent();

        ScheduleDatePicker.Date = selectedDate;
        LoadTasks();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadTasks();
    }

    private void LoadTasks()
    {
        DateLabel.Text = selectedDate.ToString("dddd, dd MMMM yyyy");

        var tasksForDate = TaskService.Tasks
            .Where(t => t.Date.Date == selectedDate.Date)
            .OrderBy(t => t.TimeText)
            .ToList();

        TasksCollectionView.ItemsSource = tasksForDate;

        int completed = tasksForDate.Count(t => t.IsCompleted);
        ProgressLabel.Text = $"{completed} / {tasksForDate.Count} Completed";
    }

    private async void OnAddTaskClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddStudyTaskPage());
    }

    private async void OnTaskSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is StudyTask selectedTask)
        {
            await Navigation.PushAsync(new TaskDetailsPage(selectedTask));
            TasksCollectionView.SelectedItem = null;
        }
    }


    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        selectedDate = e.NewDate ?? DateTime.Today;
        LoadTasks();
    }

    private void OnTodayClicked(object sender, EventArgs e)
    {
        selectedDate = DateTime.Today;
        ScheduleDatePicker.Date = selectedDate;
        LoadTasks();
    }

    private void OnTomorrowClicked(object sender, EventArgs e)
    {
        selectedDate = DateTime.Today.AddDays(1);
        ScheduleDatePicker.Date = selectedDate;
        LoadTasks();
    }
}