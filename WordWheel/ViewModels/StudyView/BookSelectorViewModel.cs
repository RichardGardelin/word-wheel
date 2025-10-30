using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using WordWheel.Models;

namespace WordWheel.ViewModels.StudyView;

public class BookSelectorViewModel : BaseViewModel
{
    private string _bookSelectionLabel = "Select Books (0 selected)";
    private SelectableBook? _selectedBook;
    private bool _isOverlayOpen;
    private bool _areAllLessonsSelected = true;

    public BookSelectorViewModel(Action closeAction)
    {
        InitializeClose(closeAction);

        ToggleAllLessonsCommand = ReactiveCommand.Create<SelectableBook>(book =>
        {
            if (book is null)
                return;
            book.ToggleAllLessons();
        });

        Books = new ObservableCollection<SelectableBook>
        {
            new("HSK 1", 15),
            new("HSK 2", 15),
            new("HSK 3", 20),
            new("HSK 4", 20),
            new("HSK 5", 0),
            new("HSK 6", 0),
        };

        foreach (var book in Books)
        {
            book.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(SelectableBook.IsSelected))
                    UpdateLabel();
            };
        }

        UpdateLabel();
    }

    public ObservableCollection<SelectableBook> Books { get; }

    public ObservableCollection<SelectableLesson> Lessons { get; } = new();

    public ReactiveCommand<SelectableBook, Unit> ToggleAllLessonsCommand { get; }

    public SelectableBook? SelectedBook
    {
        get => _selectedBook;
        set => this.RaiseAndSetIfChanged(ref _selectedBook, value);
    }

    public string BookSelectionLabel
    {
        get => _bookSelectionLabel;
        set => this.RaiseAndSetIfChanged(ref _bookSelectionLabel, value);
    }

    public bool IsOverlayOpen
    {
        get => _isOverlayOpen;
        set => this.RaiseAndSetIfChanged(ref _isOverlayOpen, value);
    }

    public bool AreAllLessonsSelected
    {
        get => _areAllLessonsSelected;
        set => this.RaiseAndSetIfChanged(ref _areAllLessonsSelected, value);
    }

    public void ToggleAllLessons()
    {
        var newState = !_areAllLessonsSelected;

        foreach (var lesson in Lessons)
        {
            lesson.IsSelected = newState;
        }

        AreAllLessonsSelected = newState;
    }

    private void UpdateLabel()
    {
        var count = Books.Count(b => b.IsSelected);
        BookSelectionLabel = $"Select Book ({count} selected)";
    }
}
