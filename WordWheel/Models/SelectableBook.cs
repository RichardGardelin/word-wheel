using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;

namespace WordWheel.Models;

public class SelectableBook : ReactiveObject
{
    private bool _isSelected;
    private bool _isLessonOverlayVisible;
    private string _name;
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
                    this.RaisePropertyChanged(nameof(LessonSummary));
            };
        }
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
        get
        {
            var selectedCount = Lessons.Count(l => l.IsSelected);

            if (IsEntireBookSelected)
                return "Entire book selected";

            if (selectedCount == 0)
                return "No lessons selected";

            return $"{selectedCount} of {Lessons.Count} selected";
        }
    }

    public void ToggleLessonOverlay()
    {
        IsLessonOverlayVisible = !IsLessonOverlayVisible;
    }

    public void ToggleAllLessons()
    {
        var newValue = !Lessons.All(l => l.IsSelected);
        foreach (var lesson in Lessons)
            lesson.IsSelected = newValue;
    }

    public bool IsEntireBookSelected =>
        (Lessons.Count > 0 && Lessons.All(l => l.IsSelected)) || Name is "HSK 5" or "HSK 6";
}
