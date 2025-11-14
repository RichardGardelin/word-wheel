using System;
using System.Reactive;
using ReactiveUI;

namespace WordWheel.ViewModels.StudyView;

public class RandomWordSelectorViewModel : BaseViewModel
{
    private int _count;

    public RandomWordSelectorViewModel()
    {
        _count = 0;

        IncreaseCountCommand = ReactiveCommand.Create(() =>
        {
            Count++;
        });
        DecreaseCountCommand = ReactiveCommand.Create(() =>
        {
            Count--;
        });
    }

    public ReactiveCommand<Unit, Unit> IncreaseCountCommand { get; }

    public ReactiveCommand<Unit, Unit> DecreaseCountCommand { get; }

    public int Count
    {
        get => _count;
        set
        {
            int clamped = Math.Clamp(value, 0, 10);
            this.RaiseAndSetIfChanged(ref _count, clamped);
        }
    }
}
