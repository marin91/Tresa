using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using Tresa.Services.Interfaces;

namespace Tresa.Services;

public class CameraService : ICameraService
{

    public CameraService()
    {

    }

    public async Task InitializeAsync(CameraView view, CancellationToken ct = default)
    {
        // permission
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
                throw new InvalidOperationException("Camera permission denied.");
        }

        var availableCameras = await view.GetAvailableCameras(ct);

        // choose back camera if available
        var back = availableCameras.FirstOrDefault(c => c.Position == CameraPosition.Rear);
        if (back is not null)
            view.SelectedCamera = back;

        // start preview on UI thread
        await MainThread.InvokeOnMainThreadAsync(() => view.StartCameraPreview(ct));
    }

    // TODO: wire to CommunityToolkit.Maui.Camera APIs or platform captures
    public async Task<byte[]?> CaptureAsync(CameraView view, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<byte[]?>(TaskCreationOptions.RunContinuationsAsynchronously);

        void Done(byte[]? bytes, Exception? ex = null)
        {
            view.MediaCaptured -= OnCaptured;
            view.MediaCaptureFailed -= OnFailed;
            if (ex is not null) tcs.TrySetException(ex);
            else if (ct.IsCancellationRequested) tcs.TrySetCanceled(ct);
            else tcs.TrySetResult(bytes);
        }

        void OnCaptured(object? s, MediaCapturedEventArgs e)
        {
            try
            {
                using var ms = new MemoryStream();
                if (e.Media.CanSeek) e.Media.Position = 0;
                e.Media.CopyTo(ms);
                Done(ms.ToArray());
            }
            catch (Exception ex) { Done(null, ex); }
        }

        void OnFailed(object? s, MediaCaptureFailedEventArgs e)
            => Done(null, new InvalidOperationException(e?.FailureReason ?? "Capture failed."));

        view.MediaCaptured += OnCaptured;
        view.MediaCaptureFailed += OnFailed;

        using var reg = ct.Register(() => Done(null));

        // v3 API: CaptureImage (not CaptureAsync)
        await MainThread.InvokeOnMainThreadAsync(() => view.CaptureImage(ct));

        return await tcs.Task.ConfigureAwait(false);
    }
}

