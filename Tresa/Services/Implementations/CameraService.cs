using CommunityToolkit.Maui.Views;
using Tresa.Services.Interfaces;

namespace Tresa.Services;

public class CameraService : ICameraService
{

    private readonly CameraView _cameraView;

    public CameraService(CameraView cameraView)
    {
        _cameraView = cameraView;
    }

    // TODO: wire to CommunityToolkit.Maui.Camera APIs or platform captures
    public async Task<byte[]?> CaptureAsync(CancellationToken cancellationToken)
    {
        // Ensure preview is running (safe to call if already started)
        try 
        { 
            await _cameraView.StartCameraPreview(cancellationToken); 
        } 
        catch(Exception e) 
        { 
            /* ignore if not needed */ 
        }

        var tcs = new TaskCompletionSource<byte[]?>(TaskCreationOptions.RunContinuationsAsynchronously);

        EventHandler<MediaCapturedEventArgs>? handler = null;
        handler = async (_, args) =>
        {
            try
            {
                // args.Media is a Stream for the captured image
                using var ms = new MemoryStream();
                await args.Media.CopyToAsync(ms).ConfigureAwait(false);
                tcs.TrySetResult(ms.ToArray());
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
            finally
            {
                // Important: unsubscribe to avoid leaks / duplicate completions
                _cameraView.MediaCaptured -= handler!;
            }
        };

        _cameraView.MediaCaptured += handler;

        try
        {
            // Triggers a single still capture
            await _cameraView.CaptureAsync();
        }
        catch (Exception ex)
        {
            // If CaptureImage() throws (e.g., permissions), make sure we clean up
            _cameraView.MediaCaptured -= handler;
            tcs.TrySetException(ex);
        }

        // Return the image bytes (or null if you prefer to signal failure differently)
        return await tcs.Task.ConfigureAwait(false);
    }
}

