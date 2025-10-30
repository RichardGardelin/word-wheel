using System;
using System.Reactive;
using ReactiveUI;

namespace WordWheel.ViewModels;

public abstract class BaseViewModel : ReactiveObject
{
    public ReactiveCommand<Unit, Unit>? CloseCommand { get; private set; }

    protected void InitializeClose(Action closeAction)
    {
        CloseCommand = ReactiveCommand.Create(closeAction);
    }
}
