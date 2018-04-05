using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Countdowner.Properties;
using MahApps.Metro;
using PublicHoliday;
using WpfUtility;

namespace Countdowner.ViewModels
{
    /// <summary>
    /// View model for the the main window
    /// </summary>
    public class SettingsWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Property to "display" wheter the set up is initial or not
        /// </summary>
        private readonly bool _initial;

        /// <summary>
        /// Property for the differen accents
        /// </summary>
        private ObservableCollection<string> _accents;

        /// <summary>
        /// Property to either include or declude public holidays
        /// </summary>
        private bool _includeHolidays;

        /// <summary>
        /// Property to either include or declude weekends
        /// </summary>
        private bool _includeWeekend;

        /// <summary>
        /// Property to set if the app should always be the top most
        /// </summary>
        private bool _isAlwaysOnTop;

        /// <summary>
        /// Property for the "last day"
        /// </summary>
        private DateTime _lastDay;

        /// <summary>
        /// Property for the remaining vacation days
        /// </summary>
        private int _remainingVacation;

        /// <summary>
        /// Property for the differen accents
        /// </summary>
        private string _selectedAccent;

        /// <summary>
        /// Property for the selected german state
        /// </summary>
        private GermanPublicHoliday.States _selectedState;

        /// <summary>
        /// Property for the selected theme
        /// </summary>
        private string _selectedTheme;

        /// <summary>
        /// Property for the differen states of germany
        /// </summary>
        private ObservableCollection<GermanPublicHoliday.States> _states;

        /// <summary>
        /// Property for the differen themes
        /// </summary>
        private ObservableCollection<string> _themes;

        /// <summary>
        /// Public constructor
        /// </summary>
        public SettingsWindowViewModel()
        {
            LastDay = Settings.Default.LastDay;
            IncludeWeekend = Settings.Default.IncludeWeekend;
            IncludeHolidays = Settings.Default.IncludeHolidays;
            IsAlwaysOnTop = Settings.Default.IsAlwaysOnTop;
            Themes = new ObservableCollection<string>(ThemeManager.AppThemes.Select(x => x.Name));
            Accents = new ObservableCollection<string>(ThemeManager.Accents.Select(x => x.Name));
            States = new ObservableCollection<GermanPublicHoliday.States>(
                ((GermanPublicHoliday.States[]) Enum.GetValues(typeof(GermanPublicHoliday.States))).Where(x =>
                    x != GermanPublicHoliday.States.ALL));
            RemainingVacation = Settings.Default.RemainingVacation;
            _initial = true;
            SelectedAccent = Settings.Default.SelectedAccent;
            SelectedTheme = Settings.Default.SelectedTheme;
            SelectedState = States.FirstOrDefault(x => x.ToString().Equals(Settings.Default.SelectedState));
            _initial = false;
        }

        /// <summary>
        /// Property for the "last day"
        /// </summary>
        public DateTime LastDay
        {
            get => _lastDay;
            set
            {
                _lastDay = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property for the remaining vacation days
        /// </summary>
        public int RemainingVacation
        {
            get => _remainingVacation;
            set
            {
                _remainingVacation = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property for the selected german state
        /// </summary>
        public GermanPublicHoliday.States SelectedState
        {
            get => _selectedState;
            set
            {
                _selectedState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property to either include or declude weekends
        /// </summary>
        public bool IncludeWeekend
        {
            get => _includeWeekend;
            set
            {
                _includeWeekend = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property to set if the app should always be the top most
        /// </summary>
        public bool IsAlwaysOnTop
        {
            get => _isAlwaysOnTop;
            set
            {
                _isAlwaysOnTop = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property to either include or declude public holidays
        /// </summary>
        public bool IncludeHolidays
        {
            get => _includeHolidays;
            set
            {
                _includeHolidays = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Command for saving the settings
        /// </summary>
        public ICommand SaveCommand => new DelegateCommand(SaveSettings);

        /// <summary>
        /// Command for switching the app style (theme and accent)
        /// </summary>
        public ICommand SwitchCommand => new DelegateCommand(SwitchAppStyle);

        /// <summary>
        /// Property for the differen accents
        /// </summary>
        public ObservableCollection<string> Accents
        {
            get => _accents;
            set => SetField(ref _accents, value);
        }

        /// <summary>
        /// Property for the differen states of germany
        /// </summary>
        public ObservableCollection<GermanPublicHoliday.States> States
        {
            get => _states;
            set => SetField(ref _states, value);
        }

        /// <summary>
        /// Property for the differen themes
        /// </summary>
        public ObservableCollection<string> Themes
        {
            get => _themes;
            set => SetField(ref _themes, value);
        }

        /// <summary>
        /// Property for the selected accent
        /// </summary>
        public string SelectedAccent
        {
            get => _selectedAccent;
            set
            {
                _selectedAccent = value;
                SwitchAppStyle();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property for the selected theme
        /// </summary>
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                _selectedTheme = value;
                SwitchAppStyle();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Changes the theme and accent to the selected ones
        /// </summary>
        private void SwitchAppStyle()
        {
            if (!_initial)
                ThemeManager.ChangeAppStyle(Application.Current,
                    ThemeManager.GetAccent(_selectedAccent),
                    ThemeManager.GetAppTheme(_selectedTheme));
        }

        /// <summary>
        /// Saves the different settings to the app settings
        /// </summary>
        private void SaveSettings()
        {
            Settings.Default.IncludeWeekend = IncludeWeekend;
            Settings.Default.IncludeHolidays = IncludeHolidays;
            Settings.Default.IsAlwaysOnTop = IsAlwaysOnTop;
            Settings.Default.SelectedState = SelectedState.ToString();
            Settings.Default.LastDay = LastDay;
            Settings.Default.RemainingVacation = RemainingVacation;
            Settings.Default.SelectedAccent = SelectedAccent;
            Settings.Default.SelectedTheme = SelectedTheme;
            Settings.Default.Save();
        }
    }
}