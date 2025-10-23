using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Tresa.Services.Interfaces;

namespace Tresa.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly IStorageService _storageService;
        private readonly INavigationService _navigationService;


        public event PropertyChangedEventHandler? PropertyChanged;


        public Models.AppSettings Settings { get; }
        public ObservableCollection<string> ColorModes { get; } = new(["Grayscale", "Contrast"]);
        public ObservableCollection<string> ExportFormats { get; } = new(["PNG", "SVG"]);


        public ICommand SaveCommand { get; }

        public SettingsViewModel(IStorageService storageService, INavigationService navigationService) 
        {
            _storageService = storageService;
            _navigationService = navigationService;

            Settings = _storageService.LoadSettings() ?? new Models.AppSettings();

            SaveCommand = new Command(async () =>
            {
                await _storageService.SaveSettingsAsync(Settings);
                await Shell.Current.DisplayAlert("Settings", "Saved.", "OK");
                await _navigationService.GoToAsync("..");
            });
        }
    }
}
