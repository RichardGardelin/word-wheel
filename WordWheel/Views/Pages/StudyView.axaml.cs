using Avalonia;
using Avalonia.Controls;
using WordWheel.Services;
using WordWheel.ViewModels.StudyView;

namespace WordWheel.Views.Pages;

public partial class StudyView : UserControl
{
    private readonly WordDataManager _wordDataManager;

    public StudyView()
    {
        InitializeComponent();
        _wordDataManager = ((App)Application.Current!).DataManager;

        DataContext = new StudyViewModel(_wordDataManager);

        if (DataContext is StudyViewModel vm)
        {
            vm.BookSelector.RequestClose += CloseBookSelector;
            vm.PosSelector.RequestClose += ClosePosSelector;
        }
    }

    private void CloseBookSelector()
    {
        if (BookSelectorButton.Flyout is Flyout flyout)
        {
            flyout.Hide();
        }
    }

    private void ClosePosSelector()
    {
        if (PosSelectorButton.Flyout is Flyout flyout)
        {
            flyout.Hide();
        }
    }
}
