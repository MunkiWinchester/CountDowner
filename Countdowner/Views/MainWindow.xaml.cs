using System.Windows;
using Countdowner.Properties;
using MahApps.Metro;

namespace Countdowner.Views
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Settings.Default.SelectedAccent = string.IsNullOrWhiteSpace(Settings.Default.SelectedAccent)
                ? "Crimson"
                : Settings.Default.SelectedAccent;
            Settings.Default.SelectedTheme = string.IsNullOrWhiteSpace(Settings.Default.SelectedTheme)
                ? "BaseDark"
                : Settings.Default.SelectedTheme;
            Settings.Default.Save();

            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(Settings.Default.SelectedAccent),
                ThemeManager.GetAppTheme(Settings.Default.SelectedTheme));
        }
    }
}