using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using WordWheel.Services;
using WordWheel.ViewModels;

namespace WordWheel.Views;

public partial class StudyView : UserControl
{
    private readonly WordDataManager _wordDataManager;

    public StudyView()
    {
        InitializeComponent();
        _wordDataManager = ((App)Application.Current!).DataManager;

        DataContext = new StudyViewModel(_wordDataManager);
    }

    private void RandomizeButton_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is StudyViewModel vm)
        {
            vm.RandomizeWords();
        }
    }
}
