using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace WordWheel.Views;

public partial class ProfileView : UserControl
{
    public event EventHandler? BackClicked;

    public ProfileView()
    {
        InitializeComponent();
        BackButton.Click += (s, e) => BackClicked?.Invoke(this, EventArgs.Empty);
    }
}
