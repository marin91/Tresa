using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tresa.Services.Interfaces;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace Tresa.ViewModels;

public class MainViewModel : ObservableObject
{
    private readonly ICameraService _cameraService;
    private readonly IStorageService _storageService;
    private readonly INavigationService _navigationService;

    private CancellationTokenSource? _captureCts;


    public IDrawable OverlayDrawable { get; }

    public IAsyncRelayCommand<CameraView> CaptureCommand { get; }
    public IAsyncRelayCommand OpenSettingsCommand { get; }
    public IAsyncRelayCommand OpenGalleryCommand { get; }


    public MainViewModel(ICameraService cameraService, IStorageService storageService, 
        INavigationService navigationService)
    {
        _cameraService = cameraService;
        _storageService = storageService;
        _navigationService = navigationService;

        OverlayDrawable = new PlaceholderEdgesDrawable();

        CaptureCommand = new AsyncRelayCommand<CameraView>(CaptureAsync);

        OpenSettingsCommand = new AsyncRelayCommand(() => _navigationService.GoToAsync("settings"));
        OpenGalleryCommand = new AsyncRelayCommand(() => _navigationService.GoToAsync("gallery"));

    }

    private async Task CaptureAsync(CameraView? cameraView)
    {
        if (cameraView is null) return;

        var bytes = await _cameraService.CaptureAsync(cameraView, CancellationToken.None);
        if (bytes is { Length: > 0 })
            await _storageService.SaveAsync(bytes);
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
