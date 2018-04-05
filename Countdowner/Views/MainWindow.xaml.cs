using System;
using System.ComponentModel;
using System.Windows;
using Countdowner.Properties;
using MahApps.Metro;

namespace Countdowner.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Top = Settings.Default.Top;
            Left = Settings.Default.Left;
            Width = Settings.Default.Width;
            Height = Settings.Default.Height;
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

            try
            {
                ThemeManager.ChangeAppStyle(Application.Current,
                    ThemeManager.GetAccent(Settings.Default.SelectedAccent),
                    ThemeManager.GetAppTheme(Settings.Default.SelectedTheme));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent("Crimson"),
                    ThemeManager.GetAppTheme("BaseDark"));
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Settings.Default.Top = Top;
            Settings.Default.Left = Left;
            Settings.Default.Width = Width;
            Settings.Default.Height = Height;
            Settings.Default.Save();
        }

        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            var window = (Window) sender;
            window.Topmost = Settings.Default.IsAlwaysOnTop;
        }
    }
}