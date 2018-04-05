using System;
using System.Linq;
using System.Timers;
using System.Windows.Input;
using Countdowner.Properties;
using Countdowner.Views;
using PublicHoliday;
using WpfUtility;

namespace Countdowner.ViewModels
{
    /// <summary>
    /// View model for the the main window
    /// </summary>
    public class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Contains the timer
        /// </summary>
        private readonly Timer _timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

        /// <summary>
        /// Boolean to display wheter the countdown is expired or not
        /// </summary>
        private bool _isDone;

        /// <summary>
        /// The timespan for the public holidays between <see langword="DateTime.Now" /> and the "last day"
        /// </summary>
        private TimeSpan _publicHolidaysSpan;

        /// <summary>
        /// The remaining days
        /// </summary>
        private int _timeLeftDays;

        /// <summary>
        /// The remaining hours
        /// </summary>
        private int _timeLeftHours;

        /// <summary>
        /// The remaining minutes
        /// </summary>
        private int _timeLeftMinutes;

        /// <summary>
        /// The remaining seconds
        /// </summary>
        private int _timeLeftSeconds;

        /// <summary>
        /// Public constructor
        /// </summary>
        public MainWindowViewModel()
        {
            _timer.Elapsed += Timer_Elapsed;
            _timer.Enabled = true;
            CalculateHolidays();
        }

        /// <summary>
        /// The remaining days
        /// </summary>
        public int TimeLeftDays
        {
            get => _timeLeftDays;
            set
            {
                _timeLeftDays = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The remaining hours
        /// </summary>
        public int TimeLeftHours
        {
            get => _timeLeftHours;
            set
            {
                _timeLeftHours = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The remaining minutes
        /// </summary>
        public int TimeLeftMinutes
        {
            get => _timeLeftMinutes;
            set
            {
                _timeLeftMinutes = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The remaining seconds
        /// </summary>
        public int TimeLeftSeconds
        {
            get => _timeLeftSeconds;
            set
            {
                _timeLeftSeconds = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Boolean to display wheter the countdown is expired or not
        /// </summary>
        public bool IsDone
        {
            get => _isDone;
            set
            {
                _isDone = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command to open the settings window
        /// </summary>
        public ICommand SettingsClickedCommand => new RelayCommand<MainWindow>(OpenSettings);

        /// <summary>
        /// Method to open the setting window
        /// </summary>
        /// <param name="mainWindow"></param>
        private void OpenSettings(MainWindow mainWindow)
        {
            var settings = new SettingsWindow {Owner = mainWindow};
            var result = settings.ShowDialog();
            if (result != null && result == true)
            {
                CalculateHolidays();
                CalculateTimer();
            }
        }

        /// <summary>
        /// Occurs when the timer elapses
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The elapsed event</param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CalculateTimer();
        }

        /// <summary>
        /// Calculates the timespan of all public holidays for the selected german state
        /// between <see langword="DateTime.Now" /> and the "last day" and sets it in the
        /// <see langword="_publicHolidaysSpan" />-property
        /// <para />
        /// Currently just works for the actual year..
        /// </summary>
        private void CalculateHolidays()
        {
            var calendar = new GermanPublicHoliday
            {
                State = ((GermanPublicHoliday.States[]) Enum.GetValues(typeof(GermanPublicHoliday.States)))
                    .FirstOrDefault(x => x.ToString().Equals(Settings.Default.SelectedState))
            };
            // TODO: handle more than the actual year?
            var publicHolidays = calendar.PublicHolidays(DateTime.Now.Year).ToList();

            var days = publicHolidays.Where(p => p >= DateTime.Now && p <= Settings.Default.LastDay).ToList();
            var totalTimeSpan = new TimeSpan();
            _publicHolidaysSpan = totalTimeSpan.Add(TimeSpan.FromDays(days.Count));
        }

        /// <summary>
        /// Calculates the remaining time and sets the corresponding properties
        /// </summary>
        private void CalculateTimer()
        {
            var ts = Settings.Default.LastDay - DateTime.Now;

            if (!Settings.Default.IncludeWeekend && ts.Days / 7 > 0)
                ts = ts.Subtract(TimeSpan.FromDays(ts.Days / 7 * 2));

            if (!Settings.Default.IncludeHolidays)
                ts = ts.Subtract(_publicHolidaysSpan);

            ts = ts.Subtract(TimeSpan.FromDays(Settings.Default.RemainingVacation));

            TimeLeftDays = ts.Days;
            TimeLeftHours = ts.Hours;
            TimeLeftMinutes = ts.Minutes;
            TimeLeftSeconds = ts.Seconds;

            if (ts.Days == 0 && ts.Hours == 0 && ts.Minutes == 0 && ts.Seconds == 0)
            {
                _timer.Stop();
                _timer.Enabled = false;
                IsDone = true;
            }
            else
            {
                _timer.Enabled = true;
                IsDone = false;
            }
        }
    }
}