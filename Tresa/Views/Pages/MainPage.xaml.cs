using CommunityToolkit.Maui.Core; // CameraInfo, CameraPosition
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using System.Threading;

namespace Tresa.Views.Pages;

public partial class MainPage : ContentPage
{
    private IReadOnlyList<CameraInfo>? _cameras;

    public MainPage() => InitializeComponent();

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await Permissions.RequestAsync<Permissions.Camera>();
        if (status == PermissionStatus.Granted)
        {
            await Camera.StartCameraPreview(CancellationToken.None); // ✅ v2.0.3
        }
        else
        {
            await DisplayAlert("Permission", "Camera permission is required.", "OK");
        }
    }

    protected override void OnDisappearing()
    {
        Camera.StopCameraPreview(); // ✅ v2.0.3
        base.OnDisappearing();
    }

    private async void OnStart(object sender, EventArgs e)
        => await Camera.StartCameraPreview(CancellationToken.None);

    private void OnStop(object sender, EventArgs e)
        => Camera.StopCameraPreview();

    private async void OnFlip(object sender, EventArgs e)
    {
        _cameras ??= await Camera.GetAvailableCameras(CancellationToken.None); // ✅
        if (_cameras is null || _cameras.Count == 0) return;

        var current = Camera.SelectedCamera;
        // Pick the opposite position
        var targetPos = current?.Position == CameraPosition.Rear ? CameraPosition.Front : CameraPosition.Rear;
        var target = _cameras.FirstOrDefault(c => c.Position == targetPos) ?? _cameras.First();

        Camera.SelectedCamera = target; // ✅ switch camera
    }

    private async void OnTorch(object sender, EventArgs e)
    {
        try
        {
            // Torch usually only works on the back camera. No explicit “supported” flag; toggle and catch if not supported.
            Camera.IsTorchOn = !Camera.IsTorchOn; // ✅ v2.0.3
        }
        catch (Exception)
        {
            await DisplayAlert("Torch", "Torch is not available on this device/camera.", "OK");
        }
    }
}
