using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Runtime.CompilerServices;
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


    public StudyViewModel(WordDataManager wordDataManager)
    {
        _wordDataManager = wordDataManager;

        RandomizeCommand = ReactiveCommand.Create(RandomizeWords);
    }

    public ReactiveCommand<Unit, Unit> RandomizeCommand { get; }

    public List<string> AvailableBooks { get; } =
    [
       "HSK1", "HSK2", "HSK3", "HSK4", "HSK5", "HSK6"
    ];

    public ObservableCollection<RandomizedWord> CurrentWords
    {
        get => _currentWords;
        set => this.RaiseAndSetIfChanged(ref _currentWords, value);
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
            PosCounts = new Dictionary<string, int>
            {
                { "Verb", 1 },
                { "Noun", 1 },
                { "Adjective", 1 }
            },
            WordRepeats = _wordRepeats
        };

        var randomizedWords = _wordDataManager.GetRandomWords(filter);

        CurrentWords = new ObservableCollection<RandomizedWord>(randomizedWords);
    }
}

