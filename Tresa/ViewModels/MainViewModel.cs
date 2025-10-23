using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tresa.Services.Interfaces;

namespace Tresa.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly ICameraService _cameraService;
    private readonly IStorageService _storageService;
    private readonly INavigationService _navigationService;

    private CancellationTokenSource? _captureCts;


    public IDrawable OverlayDrawable { get; }

    public IAsyncRelayCommand CaptureCommand { get; }
    public IAsyncRelayCommand OpenSettingsCommand { get; }
    public IAsyncRelayCommand OpenGalleryCommand { get; }


    public MainViewModel(ICameraService cameraService, IStorageService storageService, 
        INavigationService navigationService)
    {
        _cameraService = cameraService;
        _storageService = storageService;
        _navigationService = navigationService;

        OverlayDrawable = new PlaceholderEdgesDrawable();

        CaptureCommand = new AsyncRelayCommand(CaptureAsync);

        OpenSettingsCommand = new AsyncRelayCommand(() => _navigationService.GoToAsync("settings"));
        OpenGalleryCommand = new AsyncRelayCommand(() => _navigationService.GoToAsync("gallery"));

    }

    private async Task CaptureAsync()
    {
        CaptureCommand.NotifyCanExecuteChanged();

       _captureCts = new CancellationTokenSource(TimeSpan.FromSeconds(15));

        try
        {
            var bytes = await _cameraService!.CaptureAsync(_captureCts.Token);

            if (bytes is null || bytes.Length == 0)
            {   
                return;
            }

            // Prefer StorageService to return a path/string so UI can show it
            var savedPath = await _storageService!.SaveAsync(bytes);

        }
        catch (OperationCanceledException)
        {

        }
        catch (Exception ex)
        {

        }
        finally
        {            
            CaptureCommand.NotifyCanExecuteChanged();
        }
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
