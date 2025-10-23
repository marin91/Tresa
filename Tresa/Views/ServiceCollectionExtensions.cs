namespace Tresa.Views
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all MAUI <see cref="Page"/> types used in the Tresa application with the dependency injection container.
        /// </summary>
        /// <remarks>
        /// This method centralizes page registration to maintain a clean and consistent configuration within <c>MauiProgram.cs</c>.
        /// Pages are typically registered as <see cref="ServiceLifetime.Transient"/> when they are created per navigation request,
        /// and as <see cref="ServiceLifetime.Singleton"/> when they are intended to persist across the application's lifetime
        /// (for example, pages that maintain long-lived state or serve as navigation roots).
        /// </remarks>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> instance on which to register the application's pages.
        /// </param>
        /// <returns>
        /// The same <see cref="IServiceCollection"/> instance, enabling fluent chaining of service registration calls.
        /// </returns>
        public static IServiceCollection AddTresaPages(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddTransient<Pages.MainPage>();
            services.AddTransient<Pages.SettingsPage>();
            services.AddSingleton<Pages.GalleryPage>();

            return services;
        }
    }
}
