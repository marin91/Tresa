using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Tresa.Services.Interfaces;

namespace Tresa.Services;

public class CameraService : ICameraService
{

    public CameraService()
    {

    }

    // TODO: wire to CommunityToolkit.Maui.Camera APIs or platform captures
    public async Task<byte[]?> CaptureAsync(CameraView view, CancellationToken ct)
    {
        try { await view.StartCameraPreview(ct).ConfigureAwait(false); } 
        catch(Exception e)
        { 
        
        }

        var tcs = new TaskCompletionSource<byte[]?>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var reg = ct.Register(() => tcs.TrySetCanceled(ct));

        void OnCaptured(object? _, MediaCapturedEventArgs e)
        {
            view.MediaCaptured -= OnCaptured;
            using var ms = new MemoryStream();
            e.Media.CopyTo(ms);                 // sync copy keeps handler simple
            tcs.TrySetResult(ms.ToArray());
        }

        view.MediaCaptured += OnCaptured;

        try
        {
            await view.CaptureAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            view.MediaCaptured -= OnCaptured;
            tcs.TrySetException(ex);
        }

        return await tcs.Task.ConfigureAwait(false);
    }
}

