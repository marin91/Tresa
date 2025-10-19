using System.Text.Json;
using Tresa.Models;
using Tresa.Services.Interfaces;

namespace Tresa.Services.Implementations;


public class StorageService : IStorageService
{
    private readonly string _root = FileSystem.AppDataDirectory;
    private readonly string _settingsPath;


    public StorageService()
    {
        _settingsPath = Path.Combine(_root, "settings.json");
        Directory.CreateDirectory(_root);
    }


    public async Task<TraceItem> SaveAsync(byte[] pngBytes)
    {
        var file = Path.Combine(_root, $"trace_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png");
        await File.WriteAllBytesAsync(file, pngBytes);
        var item = new TraceItem { FilePath = file, CreatedUtc = DateTime.UtcNow };
        return item;
    }


    public Task<IEnumerable<TraceItem>> ListAsync()
    {
        var items = Directory.EnumerateFiles(_root, "trace_*.png")
        .Select(p => new TraceItem { FilePath = p, CreatedUtc = File.GetCreationTimeUtc(p) })
        .OrderByDescending(x => x.CreatedUtc)
        .AsEnumerable();
        return Task.FromResult(items);
    }


    public AppSettings? LoadSettings()
    {
        if (!File.Exists(_settingsPath)) return null;
        var json = File.ReadAllText(_settingsPath);
        return JsonSerializer.Deserialize<AppSettings>(json);
    }


    public Task SaveSettingsAsync(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_settingsPath, json);
        return Task.CompletedTask;
    }
}