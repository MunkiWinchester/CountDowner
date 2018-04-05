using System;
using System.ComponentModel;
using System.Windows;
using Countdowner.Properties;
using MahApps.Metro;

namespace Countdowner.Views
{
    /// <inheritdoc cref="Window" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <inheritdoc />
        /// <summary>
        /// Public constructor, sets the position and size of the window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Top = Settings.Default.Top;
            Left = Settings.Default.Left;
            Width = Settings.Default.Width;
            Height = Settings.Default.Height;
        }

        /// <summary>
        /// Event which occurs when the window is loaded
        /// <para />
        /// The method body sets the theme and accent of the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Event which occurs when the window is closed
        /// <para />
        /// The method body saves the size and position of the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Settings.Default.Top = Top;
            Settings.Default.Left = Left;
            Settings.Default.Width = Width;
            Settings.Default.Height = Height;
            Settings.Default.Save();
        }

        /// <summary>
        /// Event which occurs when the window is deactivated
        /// <para />
        /// The method body sets the window to the top most if it should be
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnDeactivated(object sender, EventArgs e)
        {
            var window = (Window) sender;
            window.Topmost = Settings.Default.IsAlwaysOnTop;
        }
    }
}