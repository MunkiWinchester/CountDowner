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
    public class SettingsWindowViewModel : ObservableObject
    {
        private readonly bool _initial;

        private ObservableCollection<string> _accents;
        private bool _includeHolidays;

        private bool _includeWeekend;

        private bool _isAlwaysOnTop;
        private DateTime _lastDay;

        private int _remainingVacation;

        private string _selectedAccent;

        private GermanPublicHoliday.States _selectedState;

        private string _selectedTheme;
        private ObservableCollection<GermanPublicHoliday.States> _states;

        private ObservableCollection<string> _themes;

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

        public DateTime LastDay
        {
            get => _lastDay;
            set
            {
                _lastDay = value;
                OnPropertyChanged();
            }
        }

        public int RemainingVacation
        {
            get => _remainingVacation;
            set
            {
                _remainingVacation = value;
                OnPropertyChanged();
            }
        }

        public GermanPublicHoliday.States SelectedState
        {
            get => _selectedState;
            set
            {
                _selectedState = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeWeekend
        {
            get => _includeWeekend;
            set
            {
                _includeWeekend = value;
                OnPropertyChanged();
            }
        }

        public bool IsAlwaysOnTop
        {
            get => _isAlwaysOnTop;
            set
            {
                _isAlwaysOnTop = value;
                OnPropertyChanged();
            }
        }

        public bool IncludeHolidays
        {
            get => _includeHolidays;
            set
            {
                _includeHolidays = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand => new DelegateCommand(SaveSettings);

        public ICommand SwitchCommand => new DelegateCommand(SwitchAppStyle);

        public ObservableCollection<string> Accents
        {
            get => _accents;
            set => SetField(ref _accents, value);
        }

        public ObservableCollection<GermanPublicHoliday.States> States
        {
            get => _states;
            set => SetField(ref _states, value);
        }

        public ObservableCollection<string> Themes
        {
            get => _themes;
            set => SetField(ref _themes, value);
        }

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

        private void SwitchAppStyle()
        {
            if (!_initial)
                ThemeManager.ChangeAppStyle(Application.Current,
                    ThemeManager.GetAccent(_selectedAccent),
                    ThemeManager.GetAppTheme(_selectedTheme));
        }

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