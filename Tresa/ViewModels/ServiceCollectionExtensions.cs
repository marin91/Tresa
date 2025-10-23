using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tresa.Services.Implementations;

namespace Tresa.ViewModels
{
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all ViewModel types used throughout the Tresa application with the dependency injection container.
        /// </summary>
        /// <remarks>
        /// This method centralizes ViewModel registration to keep <c>MauiProgram.cs</c> clean and consistent.
        /// ViewModels are typically registered as <see cref="ServiceLifetime.Transient"/> because they are
        /// instantiated per page or navigation request and do not maintain global state.
        /// </remarks>
        /// <param name="services">
        /// The <see cref="IServiceCollection"/> instance on which to register the ViewModels.
        /// </param>
        /// <returns>
        /// The same <see cref="IServiceCollection"/> instance to enable fluent configuration chaining.
        /// </returns>
        public static IServiceCollection AddTresaViewModels(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsViewModel>();

            return services;
        }
    }
}
