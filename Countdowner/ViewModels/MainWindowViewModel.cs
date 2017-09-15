﻿using System;
using System.Timers;
using System.Windows.Input;
using Countdowner.Properties;
using Countdowner.Views;
using WpfUtility;

namespace Countdowner.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        ///     Contains the timer
        /// </summary>
        private readonly Timer _timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

        private bool _isDone;

        private int _timeLeftDays;
        private int _timeLeftHours;
        private int _timeLeftMinutes;
        private int _timeLeftSeconds;

        public MainWindowViewModel()
        {
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
        }

        public int TimeLeftDays
        {
            get => _timeLeftDays;
            set
            {
                _timeLeftDays = value;
                OnPropertyChanged();
            }
        }

        public int TimeLeftHours
        {
            get => _timeLeftHours;
            set
            {
                _timeLeftHours = value;
                OnPropertyChanged();
            }
        }

        public int TimeLeftMinutes
        {
            get => _timeLeftMinutes;
            set
            {
                _timeLeftMinutes = value;
                OnPropertyChanged();
            }
        }

        public int TimeLeftSeconds
        {
            get => _timeLeftSeconds;
            set
            {
                _timeLeftSeconds = value;
                OnPropertyChanged();
            }
        }

        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                OnPropertyChanged();
            }
        }

        public ICommand SettingsClickedCommand => new RelayCommand<MainWindow>(OpenSettings);

        private static void OpenSettings(MainWindow mainWindow)
        {
            var settings = new SettingsWindow {Owner = mainWindow};
            settings.ShowDialog();
        }

        /// <summary>
        ///     Occurs when the timer elapsed
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The elapsed event</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var ts = Settings.Default.LastDay - DateTime.Now;
            if (!Settings.Default.IncludeWeekend && ts.Days / 7 > 0)
                ts = ts.Subtract(TimeSpan.FromDays(ts.Days / 7 * 2));

            TimeLeftDays = ts.Days;
            TimeLeftHours = ts.Hours;
            TimeLeftMinutes = ts.Minutes;
            TimeLeftSeconds = ts.Seconds;
            if (ts == TimeSpan.Zero)
                IsDone = true;
            //TimeLeft = string.Format("{0:%d} days\r\n{0:%h} hours\r\n{0:%m} minutes\r\n{0:%s} seconds", ts);
        }
    }
}