using ReactiveUI;

namespace WordWheel.Models;

public class SelectableLesson : ReactiveObject
{
    private bool _isSelected;

    public SelectableLesson(string name)
    {
        Name = name;
        _isSelected = true;
    }

    public string Name { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }
}
