using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tresa.Services.Interfaces;

namespace Tresa.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly ICameraService? _camera;
    private readonly IStorageService? _storage;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public IDrawable OverlayDrawable { get; }

    public ICommand CaptureCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ICommand OpenGalleryCommand { get; }


    public MainViewModel(ICameraService camera, IStorageService storage)
    {
        _camera = camera;
        _storage = storage;

        OverlayDrawable = new PlaceholderEdgesDrawable();

        CaptureCommand = new Command(async () =>
        {
            if (_camera is null || _storage is null) return; // XAML ctor path: skip
            var bytes = await _camera.CaptureAsync();
            if (bytes is not null)
                await _storage.SaveAsync(bytes);
        });

        OpenSettingsCommand = new Command(async () => await Shell.Current.GoToAsync("settings"));
        OpenGalleryCommand = new Command(async () => await Shell.Current.GoToAsync("gallery"));
    }

    // Simple placeholder for now; replace with your real drawable
    private sealed class PlaceholderEdgesDrawable : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Lime;
            canvas.StrokeSize = 2;
            canvas.DrawRectangle(dirtyRect.Inflate(SizeF.Zero));
        }
    }
}
