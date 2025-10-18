using Tresa.Models;

namespace Tresa.Services.Interfaces
{
    public interface IStorageService
    {
        Task<TraceItem> SaveAsync(byte[] pngBytes);
        Task<IEnumerable<TraceItem>> ListAsync();


        AppSettings? LoadSettings();
        Task SaveSettingsAsync(AppSettings settings);
    }
}
