namespace Tresa.Services.Interfaces
{
    public interface INavigationService
    {
        Task GoToAsync(string route,
                       IDictionary<string, object>? parameters = null,
                       CancellationToken ct = default);
    }
}
