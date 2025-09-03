using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace WordWheel.Views;

public partial class StudyView : UserControl
{
    public event EventHandler? BackClicked;

    public StudyView()
    {
        InitializeComponent();
        BackButton.Click += (s, e) => BackClicked?.Invoke(this, EventArgs.Empty);
    }
}
