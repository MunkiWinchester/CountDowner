using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Countdowner.Properties;
using MahApps.Metro;
using WpfUtility;

namespace Countdowner.ViewModels
{
    public class SettingsWindowViewModel : ObservableObject
    {
        private readonly bool _initial;

        private ObservableCollection<string> _accents;

        private bool _includeWeekend;
        private DateTime _lastDay;

        private string _selectedAccent;

        private string _selectedTheme;

        private ObservableCollection<string> _themes;

        public SettingsWindowViewModel()
        {
            LastDay = Settings.Default.LastDay;
            IncludeWeekend = Settings.Default.IncludeWeekend;
            Themes = new ObservableCollection<string>(ThemeManager.AppThemes.Select(x => x.Name));
            Accents = new ObservableCollection<string>(ThemeManager.Accents.Select(x => x.Name));
            _initial = true;
            SelectedAccent = Settings.Default.SelectedAccent;
            SelectedTheme = Settings.Default.SelectedTheme;
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

        public bool IncludeWeekend
        {
            get => _includeWeekend;
            set
            {
                _includeWeekend = value;
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
            Settings.Default.LastDay = LastDay;
            Settings.Default.SelectedAccent = SelectedAccent;
            Settings.Default.SelectedTheme = SelectedTheme;
            Settings.Default.Save();
        }
    }
}