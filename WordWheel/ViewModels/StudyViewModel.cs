using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using WordWheel.Models;
using WordWheel.Services;

namespace WordWheel.ViewModels;

public class StudyViewModel : INotifyPropertyChanged
{
    private readonly WordDataManager _wordDataManager;
    private ObservableCollection<RandomizedWord> _currentWords = [];
    private bool _hideEnglish;
    private bool _hidePinyin;
    private bool _hidePos;
    private string _selectedBook = "";
    private bool _wordRepeats;


    public StudyViewModel(WordDataManager wordDataManager)
    {
        _wordDataManager = wordDataManager;
    }

    public List<string> AvailableBooks { get; } =
    [
       "HSK1", "HSK2", "HSK3", "HSK4", "HSK5", "HSK6"
    ];

    public ObservableCollection<RandomizedWord> CurrentWords
    {
        get => _currentWords;
        set
        {
            if (_currentWords != value)
            {
                _currentWords = value;
                OnPropertyChanged(nameof(CurrentWords));
            }
        }
    }

    public bool HideEnglish
    {
        get => _hideEnglish;
        set
        {
            if (_hideEnglish != value)
            {
                _hideEnglish = value;
                OnPropertyChanged(nameof(HideEnglish));
            }
        }
    }

    public bool HidePinyin
    {
        get => _hidePinyin;
        set
        {
            if (_hidePinyin != value)
            {
                _hidePinyin = value;
                OnPropertyChanged(nameof(HidePinyin));
            }
        }
    }

    public bool HidePos
    {
        get => _hidePos;
        set
        {
            if (_hidePos != value)
            {
                _hidePos = value;
                OnPropertyChanged(nameof(HidePos));
            }
        }
    }

    public bool WordRepeats
    {
        get => _wordRepeats;
        set
        {
            if (_wordRepeats != value)
            {
                _wordRepeats = value;
                OnPropertyChanged(nameof(WordRepeats));
            }
        }
    }

    public string SelectedBook
    {
        get => _selectedBook;
        set
        {
            if (_selectedBook != value)
            {
                _selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }
    }

    public void RandomizeWords()
    {
        var bookName = SelectedBook + ".json";

        var filter = new WordFilter
        {
            Books = [bookName],
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

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

