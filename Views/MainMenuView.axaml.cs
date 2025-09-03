using Avalonia.Controls;
using System;

namespace WordWheel.Views;

public partial class MainMenuView : UserControl
{
    public event EventHandler? ProfileClicked;
    public event EventHandler? StudyClicked;

    public MainMenuView()
    {
        InitializeComponent();

        ProfileButton.Click += (s, e) => ProfileClicked?.Invoke(this, EventArgs.Empty);
        StudyButton.Click += (s, e) => StudyClicked?.Invoke(this, EventArgs.Empty);
    }
}
