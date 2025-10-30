using System;
using ReactiveUI;

namespace WordWheel.Models;

public class SelectablePOS : ReactiveObject
{
    private bool _isSelected;
    private int _count;

    public SelectablePOS(string name)
    {
        Name = name;
        _count = 1;
    }

    public string Name { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
    }

    public int Count
    {
        get => _count;
        set
        {
            int clamped = Math.Clamp(value, 1, 10);
            this.RaiseAndSetIfChanged(ref _count, clamped);
        }
    }
}
