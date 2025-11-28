using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia;
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
    private string _fullSelectionSummary = "";
    private int _availableWordsCount;
    private int _wordsToDraw;

    public StudyViewModel(WordDataManager wordDataManager)
    {
        _wordDataManager = wordDataManager;

        BookSelector = new BookSelectorViewModel(() => IsBookSelectorOpen = false);
        PosSelector = new PosSelectorViewModel(() => IsPosSelectorOpen = false);
        RandomWordSelector = new RandomWordSelectorViewModel();

        RandomizeCommand = ReactiveCommand.Create(RandomizeWords);

        foreach (var book in BookSelector.Books)
        {
            book.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(SelectableBook.IsSelected))
                {
                    UpdateAvailableWordsCount();
                    BuildSummary();
                }
            };

            foreach (var lesson in book.Lessons)
            {
                lesson.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == nameof(SelectableLesson.IsSelected))
                    {
                        UpdateAvailableWordsCount();
                        BuildSummary();
                    }
                };
            }
        }

        foreach (var pos in PosSelector.PosOptions)
        {
            pos.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(SelectablePOS.IsSelected))
                {
                    UpdateAvailableWordsCount();
                    UpdateWordsToDraw();
                    BuildSummary();
                }

                if (e.PropertyName == nameof(SelectablePOS.Count))
                {
                    UpdateWordsToDraw();
                }
            };
        }

        RandomWordSelector.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(RandomWordSelector.Count))
            {
                BuildSummary();
                UpdateWordsToDraw();
            }
        };

        BuildSummary();
    }

    public BookSelectorViewModel BookSelector { get; }

    public PosSelectorViewModel PosSelector { get; }

    public RandomWordSelectorViewModel RandomWordSelector { get; }

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

    public int AvailableWordsCount
    {
        get => _availableWordsCount;
        private set => this.RaiseAndSetIfChanged(ref _availableWordsCount, value);
    }

    public int WordsToDraw
    {
        get => _wordsToDraw;
        set => this.RaiseAndSetIfChanged(ref _wordsToDraw, value);
    }

    public string FullSelectionSummary
    {
        get => _fullSelectionSummary;
        private set => this.RaiseAndSetIfChanged(ref _fullSelectionSummary, value);
    }

    private void RandomizeWords()
    {
        var filter = BuildFilter();
        var randomizedWords = _wordDataManager.GetRandomWords(filter);

        CurrentWords.Clear();
        foreach (var word in randomizedWords)
            CurrentWords.Add(word);
    }

    private void UpdateAvailableWordsCount()
    {
        var filter = BuildFilter();
        AvailableWordsCount = _wordDataManager.GetFilteredWordCount(filter);
    }

    private void UpdateWordsToDraw()
    {
        var totalPos = PosSelector.PosOptions.Where(p => p.IsSelected).Sum(p => p.Count);
        WordsToDraw = RandomWordSelector.Count + totalPos;
    }

    private void BuildSummary()
    {
        var noBooksSelected = BookSelector.Books.All(b => !b.IsSelected);
        var noPosSelected = PosSelector.PosOptions.All(p => !p.IsSelected);
        int drawCount = RandomWordSelector.Count;

        if (noBooksSelected)
        {
            FullSelectionSummary = "No books selected";
            return;
        }

        if (drawCount == 0 && noPosSelected)
        {
            FullSelectionSummary = $"{BookSelector.SelectionSummary}\nSelect POS or random word";
            return;
        }

        FullSelectionSummary = string.Join(
            "\n",
            new[]
            {
                BookSelector.SelectionSummary,
                PosSelector.SelectionSummary,
                $"Random words selected: {drawCount}",
            }
        );
    }

    private WordFilter BuildFilter()
    {
        var filter = new WordFilter
        {
            Books = [.. BookSelector.Books.Where(b => b.IsSelected).Select(b => b.Name)],
            AllLessonsBooks =
            [
                .. BookSelector
                    .Books.Where(b => b.IsSelected && b.IsEntireBookSelected)
                    .Select(b => b.Name),
            ],
            Lessons = BookSelector
                .Books.Where(b => b.IsSelected)
                .ToDictionary(
                    b => b.Name,
                    b =>
                        b.Lessons.Where(l => l.IsSelected)
                            .Select(l => int.Parse(l.Name.Replace("Lesson ", string.Empty)))
                            .ToHashSet()
                ),
            PosCounts = PosSelector
                .PosOptions.Where(p => p.IsSelected)
                .ToDictionary(p => p.Name, p => p.Count),
            WordRepeats = WordRepeats,
        };

        // Add Random if selected
        if (RandomWordSelector.Count > 0)
            filter.PosCounts["Random"] = RandomWordSelector.Count;

        return filter;
    }
}
