using Avalonia.Controls;
using WordWheel.Views;

namespace WordWheel;

public partial class MainWindow : Window
{
    private StudyView _studyView;
    private ProfileView _profileView;

    public MainWindow()
    {
        InitializeComponent();

        // Initialize views
        _studyView = new StudyView();
        _profileView = new ProfileView();

        // Wire up events
        _profileView.BackClicked += (_, _) => ShowStudyView();
        _studyView.BackClicked += (_, _) => ShowProfileView();

        // Show main menu on start
        ShowStudyView();
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