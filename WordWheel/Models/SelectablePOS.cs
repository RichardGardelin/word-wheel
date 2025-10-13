using System;
using ReactiveUI;

namespace WordWheel.Models;

public class SelectablePOS(string name) : ReactiveObject
{
    private bool _isSelected;
    private int _count = 1;

    public string Name { get; } = name;

    public Action? OnSelectionChanged { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            this.RaiseAndSetIfChanged(ref _isSelected, value);
            OnSelectionChanged?.Invoke();
        }
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
