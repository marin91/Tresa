using CommunityToolkit.Maui.Views;
using Tresa.Services.Interfaces;

namespace Tresa.Services;

public class CameraService : ICameraService
{

    public CameraService()
    {

    }

    // TODO: wire to CommunityToolkit.Maui.Camera APIs or platform captures
    public async Task<byte[]?> CaptureAsync(CameraView cameraView, CancellationToken cancellationToken)
    {
        // Ensure preview is running (safe to call if already started)
        try 
        { 
            await cameraView.StartCameraPreview(cancellationToken); 
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
                cameraView.MediaCaptured -= handler!;
            }
        };

        cameraView.MediaCaptured += handler;

        try
        {
            // Triggers a single still capture
            await cameraView.CaptureAsync();
        }
        catch (Exception ex)
        {
            // If CaptureImage() throws (e.g., permissions), make sure we clean up
            cameraView.MediaCaptured -= handler;
            tcs.TrySetException(ex);
        }

        // Return the image bytes (or null if you prefer to signal failure differently)
        return await tcs.Task.ConfigureAwait(false);
    }
}

