using System;
using System.Linq;
using Avalonia.Controls;

namespace word_wheel;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnShuffleClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var input = InputText.Text;
        if (string.IsNullOrWhiteSpace(input))
        {
            OutputText.Text = "Please enter some text.";
            return;
        }

        var random = new Random();
        var shuffled = new string(input.OrderBy(_ => random.Next()).ToArray());
        OutputText.Text = $"Shuffled: {shuffled}";
    }
}