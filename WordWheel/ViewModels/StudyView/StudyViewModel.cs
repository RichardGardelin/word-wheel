using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using ReactiveUI;
using WordWheel.Models;
using WordWheel.Services;

namespace WordWheel.ViewModels.StudyView;

public class StudyViewModel : BaseViewModel
{
    private readonly WordDataManager _wordDataManager;
    private bool _hideEnglish;
    private bool _hidePinyin;
    private bool _hidePos;
    private bool _wordRepeats;
    private bool _isPosSelectorOpen;
    private bool _isBookSelectorOpen;

    public StudyViewModel(WordDataManager wordDataManager)
    {
        _wordDataManager = wordDataManager;

        BookSelector = new BookSelectorViewModel(() => IsBookSelectorOpen = false);
        PosSelector = new PosSelectorViewModel(() => IsPosSelectorOpen = false);

        RandomizeCommand = ReactiveCommand.Create(RandomizeWords);
    }

    public BookSelectorViewModel BookSelector { get; }
    public PosSelectorViewModel PosSelector { get; }

    public ObservableCollection<RandomizedWord> CurrentWords { get; } = new();

    public ReactiveCommand<Unit, Unit> RandomizeCommand { get; }

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

    public bool IsPosSelectorOpen
    {
        get => _isPosSelectorOpen;
        set { this.RaiseAndSetIfChanged(ref _isPosSelectorOpen, value); }
    }

    public bool IsBookSelectorOpen
    {
        get => _isBookSelectorOpen;
        set { this.RaiseAndSetIfChanged(ref _isBookSelectorOpen, value); }
    }

    private void RandomizeWords()
    {
        var filter = new WordFilter
        {
            Books = [$"{BookSelector.SelectedBook?.Name}.json"],
            PosCounts = PosSelector
                .PosOptions.Where(p => p.IsSelected)
                .ToDictionary(p => p.Name, p => p.Count),
            WordRepeats = WordRepeats,
        };

        var randomizedWords = _wordDataManager.GetRandomWords(filter);

        CurrentWords.Clear();
        foreach (var word in randomizedWords)
            CurrentWords.Add(word);
    }
}
