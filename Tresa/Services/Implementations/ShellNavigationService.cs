using Tresa.Services.Interfaces;

namespace Tresa.Services.Implementations
{
    internal class ShellNavigationService : INavigationService
    {
        public Task GoToAsync(string route, IDictionary<string, object>? parameters = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(route))
                throw new ArgumentException("Route is required.", nameof(route));

            var shell = Shell.Current ?? throw new InvalidOperationException("Shell.Current is null.");

            Task Core() => parameters is null ? shell.GoToAsync(route) : shell.GoToAsync(route, parameters);

            return MainThread.IsMainThread
                ? Core()
                : MainThread.InvokeOnMainThreadAsync(Core);
        }
    }
}
