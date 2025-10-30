using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;

namespace WordWheel.Models;

public class SelectableBook : ReactiveObject
{
    private bool _isSelected;
    private bool _isLessonOverlayVisible;
    private string _name;
    private string _lessonSummary = "";
    private ObservableCollection<SelectableLesson> _lessons;

    public SelectableBook(string name, int lessonCount)
    {
        _name = name;
        _lessons = new ObservableCollection<SelectableLesson>(
            Enumerable.Range(1, lessonCount).Select(i => new SelectableLesson($"Lesson {i}"))
        );

        foreach (var lesson in _lessons)
        {
            lesson.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(SelectableLesson.IsSelected))
                    UpdateLessonSummary();
            };
        }

        UpdateLessonSummary();
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }

    public bool IsLessonOverlayVisible
    {
        get => _isLessonOverlayVisible;
        set => this.RaiseAndSetIfChanged(ref _isLessonOverlayVisible, value);
    }

    public ObservableCollection<SelectableLesson> Lessons
    {
        get => _lessons;
        set => this.RaiseAndSetIfChanged(ref _lessons, value);
    }

    public string LessonSummary
    {
        get => _lessonSummary;
        private set => this.RaiseAndSetIfChanged(ref _lessonSummary, value);
    }

    public void ToggleLessonOverlay()
    {
        IsLessonOverlayVisible = !IsLessonOverlayVisible;
    }

    public void ToggleAllLessons()
    {
        var newValue = !Lessons.All(l => l.IsSelected);
        foreach (var lesson in Lessons)
        {
            lesson.IsSelected = newValue;
        }

        UpdateLessonSummary();
    }

    private void UpdateLessonSummary()
    {
        var selectedCount = Lessons.Count(l => l.IsSelected);

        if (selectedCount == Lessons.Count && Lessons.Count > 0)
            LessonSummary = "Entire book selected";
        else if (selectedCount == 0)
        {
            if (Name is "HSK 5" or "HSK 6")
                LessonSummary = "Entire book selected";
            else
                LessonSummary = "No lessons selected";
        }
        else
            LessonSummary = $"{selectedCount} of {Lessons.Count} selected";
    }
}
