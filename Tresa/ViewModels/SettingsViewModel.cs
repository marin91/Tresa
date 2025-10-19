using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Tresa.Services.Interfaces;

namespace Tresa.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly IStorageService _storage;


        public event PropertyChangedEventHandler? PropertyChanged;


        public Models.AppSettings Settings { get; }
        public ObservableCollection<string> ColorModes { get; } = new(["Grayscale", "Contrast"]);
        public ObservableCollection<string> ExportFormats { get; } = new(["PNG", "SVG"]);


        public ICommand SaveCommand { get; }

        public SettingsViewModel(IStorageService storage) 
        {
            _storage = storage;

            Settings = _storage.LoadSettings() ?? new Models.AppSettings();

            SaveCommand = new Command(async () =>
            {
                await _storage.SaveSettingsAsync(Settings);
                await Shell.Current.DisplayAlert("Settings", "Saved.", "OK");
                await Shell.Current.GoToAsync("..");
            });
        }
    }
}
