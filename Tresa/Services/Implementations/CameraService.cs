using Tresa.Services.Interfaces;

namespace Tresa.Services;

public class CameraService : ICameraService
{
    // TODO: wire to CommunityToolkit.Maui.Camera APIs or platform captures
    public Task<byte[]?> CaptureAsync()
    {
        // Placeholder: return null until implemented
        return Task.FromResult<byte[]?>(null);
    }
}

