using System;
using Avalonia.Controls;

namespace WordWheel.Views.Pages;

public partial class ProfileView : UserControl
{
    public event EventHandler? BackClicked;

    public ProfileView()
    {
        InitializeComponent();
        StudyButton.Click += (s, e) => BackClicked?.Invoke(this, EventArgs.Empty);
    }
}
