using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using WordWheel.Models;
using WordWheel.Services;

namespace WordWheel.ViewModels;

public class StudyViewModel : ReactiveObject
{
    private readonly WordDataManager _wordDataManager;

    private ObservableCollection<RandomizedWord> _currentWords = [];
    private bool _hideEnglish;
    private bool _hidePinyin;
    private bool _hidePos;
    private bool _wordRepeats;
    private string _selectedBook = "";
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

    public StudyViewModel(WordDataManager wordDataManager)
    {
        _wordDataManager = wordDataManager;

        RandomizeCommand = ReactiveCommand.Create(RandomizeWords);

        IncreaseCountCommand = ReactiveCommand.Create<SelectablePOS>(pos => pos.Count++);
        DecreaseCountCommand = ReactiveCommand.Create<SelectablePOS>(pos => pos.Count--);

        PosOptions = new ObservableCollection<SelectablePOS>(
            AllPOSNames.Select(name =>
            {
                var pos = new SelectablePOS(name) { OnSelectionChanged = UpdatePosSelectionLabel };
                return pos;
            })
        );
        UpdatePosSelectionLabel();
    }

    public ReactiveCommand<SelectablePOS, Unit> IncreaseCountCommand { get; }

    public ReactiveCommand<SelectablePOS, Unit> DecreaseCountCommand { get; }

    public ReactiveCommand<Unit, Unit> RandomizeCommand { get; }

    public List<string> AvailableBooks { get; } = ["HSK1", "HSK2", "HSK3", "HSK4", "HSK5", "HSK6"];

    public ObservableCollection<SelectablePOS> PosOptions { get; }

    public ObservableCollection<RandomizedWord> CurrentWords
    {
        get => _currentWords;
        set => this.RaiseAndSetIfChanged(ref _currentWords, value);
    }

    public string PosSelectionLabel
    {
        get => _posSelectionLabel;
        set => this.RaiseAndSetIfChanged(ref _posSelectionLabel, value);
    }

    public bool HideEnglish
    {
        get => _hideEnglish;
        set => this.RaiseAndSetIfChanged(ref _hideEnglish, value);
    }

    public bool HidePinyin
    {
        get => _hidePinyin;
        set => this.RaiseAndSetIfChanged(ref _hidePinyin, value);
    }

    public bool HidePos
    {
        get => _hidePos;
        set => this.RaiseAndSetIfChanged(ref _hidePos, value);
    }

    public bool WordRepeats
    {
        get => _wordRepeats;
        set => this.RaiseAndSetIfChanged(ref _wordRepeats, value);
    }

    public string SelectedBook
    {
        get => _selectedBook;
        set => this.RaiseAndSetIfChanged(ref _selectedBook, value);
    }

    public void RandomizeWords()
    {
        var filter = new WordFilter
        {
            Books = [$"{SelectedBook}.json"],
            PosCounts = PosOptions.Where(p => p.IsSelected).ToDictionary(p => p.Name, p => p.Count),
            WordRepeats = _wordRepeats,
        };

        var randomizedWords = _wordDataManager.GetRandomWords(filter);

        CurrentWords = new ObservableCollection<RandomizedWord>(randomizedWords);
    }

    private void UpdatePosSelectionLabel()
    {
        var selectedCount = PosOptions.Count(p => p.IsSelected);
        PosSelectionLabel = $"Select POS ({selectedCount} selected)";
    }
}
