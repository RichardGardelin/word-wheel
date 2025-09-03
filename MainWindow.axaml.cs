using Avalonia.Controls;
using WordWheel.Views;

namespace WordWheel;

public partial class MainWindow : Window
{
    private MainMenuView _mainMenuView;
    private ProfileView _profileView;
    private StudyView _studyView;

    public MainWindow()
    {
        InitializeComponent();

        // Initialize views
        _mainMenuView = new MainMenuView();
        _profileView = new ProfileView();
        _studyView = new StudyView();

        // Wire up events
        _mainMenuView.ProfileClicked += (_, _) => ShowProfileView();
        _mainMenuView.StudyClicked += (_, _) => ShowStudyView();

        _profileView.BackClicked += (_, _) => ShowMainMenu();
        _studyView.BackClicked += (_, _) => ShowMainMenu();

        // Show main menu on start
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        MainContent.Content = _mainMenuView;
    }

    private void ShowProfileView()
    {
        MainContent.Content = _profileView;
    }

    private void ShowStudyView()
    {
        MainContent.Content = _studyView;
    }
}