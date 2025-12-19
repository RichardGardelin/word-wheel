using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using WordWheel.Models;

namespace WordWheel.ViewModels.StudyView;

public class PosSelectorViewModel : BaseViewModel
{
    private string _posSelectionLabel = "Select POS (0 selected)";
    private static readonly string[] AllPOSNames =
    [
        "Verb",
        "Noun",
        "Adjective",
        "Adverb",
        "Conjunction",
        "Preposition",
        "Pronoun",
        "Measure word",
        "Directional",
        "Time word",
        "Numeral",
        "Location word",
        "Fixed term",
        "Particle",
    ];

    public PosSelectorViewModel()
    {
        RequestCloseCommand = ReactiveCommand.Create(() =>
        {
            RequestClose?.Invoke();
        });

        IncreaseCountCommand = ReactiveCommand.Create<SelectablePOS>(pos => pos.Count++);
        DecreaseCountCommand = ReactiveCommand.Create<SelectablePOS>(pos => pos.Count--);

        PosOptions = new ObservableCollection<SelectablePOS>(
            AllPOSNames.Select(name => new SelectablePOS(name))
        );

        foreach (var pos in PosOptions)
        {
            pos.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(SelectablePOS.IsSelected))
                    UpdateLabel();
            };
        }

        UpdateLabel();
    }

    public ObservableCollection<SelectablePOS> PosOptions { get; }

    public ReactiveCommand<SelectablePOS, Unit> IncreaseCountCommand { get; }

    public ReactiveCommand<SelectablePOS, Unit> DecreaseCountCommand { get; }

    public event Action? RequestClose;

    public ReactiveCommand<Unit, Unit> RequestCloseCommand { get; }

    public string SelectionSummary
    {
        get => ComputeSelectionSummary();
    }

    public string PosSelectionLabel
    {
        get => _posSelectionLabel;
        set => this.RaiseAndSetIfChanged(ref _posSelectionLabel, value);
    }

    private string ComputeSelectionSummary()
    {
        var selected = PosOptions.Where(p => p.IsSelected).ToList();
        if (selected.Count == 0)
            return "No POS selected";

        if (selected.Count < 4)
        {
            var names = selected.Select(p => p.Name);
            return "Selected POS: " + string.Join(", ", names);
        }

        return $"{selected.Count} different POS selected";
    }

    private void UpdateLabel()
    {
        var selectedCount = PosOptions.Count(p => p.IsSelected);
        PosSelectionLabel = $"Select POS ({selectedCount} selected)";
    }
}
