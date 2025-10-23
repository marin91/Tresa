using Tresa.Services.Implementations;
using Tresa.Services.Interfaces;

namespace Tresa.Services
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all application services, including platform abstractions and navigation helpers.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
        /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddTresaServices(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            // Core app services
            services.AddTransient<IStorageService, StorageService>();
            services.AddTransient<ICameraService, CameraService>();
            services.AddSingleton<INavigationService, ShellNavigationService>();

            return services;
        }
    }
}
