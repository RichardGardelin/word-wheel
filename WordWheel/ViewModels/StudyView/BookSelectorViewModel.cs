using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
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

    public string SelectionSummary
    {
        get => ComputeSelectionSummary();
    }

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

    private string ComputeSelectionSummary()
    {
        var selectedBooks = Books.Where(b => b.IsSelected).ToList();
        if (selectedBooks.Count == 0)
            return "No books selected";

        if (selectedBooks.Count == 1)
        {
            var book = selectedBooks[0];
            var selectedLessons = book.Lessons.Where(l => l.IsSelected).ToList();

            if (selectedLessons.Count == book.Lessons.Count)
                return $"Entire {book.Name} selected";

            if (selectedLessons.Count == 1)
                return $"{selectedLessons[0].Name} in {book.Name} selected";

            return $"{selectedLessons.Count} lessons in {book.Name} selected";
        }

        int totalLessons = selectedBooks.Sum(b => b.Lessons.Count(l => l.IsSelected));

        string bookWord = selectedBooks.Count == 1 ? "book" : "books";
        string lessonWord = totalLessons == 1 ? "lesson" : "lessons";

        return $"{selectedBooks.Count} {bookWord} with {totalLessons} {lessonWord} selected";
    }

    private void UpdateLabel()
    {
        var count = Books.Count(b => b.IsSelected);
        BookSelectionLabel = $"Select Book ({count} selected)";
    }
}
