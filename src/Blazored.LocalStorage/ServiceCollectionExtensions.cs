using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Blazored.LocalStorage
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazoredLocalStorage(this IServiceCollection services)
            => AddBlazoredLocalStorage(services, null);

        public static IServiceCollection AddBlazoredLocalStorage(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            services.TryAddScoped<IStorageProvider, BrowserStorageProvider>();
            AddServices(services, configure);
            return services;
        }

        public static IServiceCollection AddBlazoredLocalStorageStreaming(this IServiceCollection services)
            => AddBlazoredLocalStorageStreaming(services, null);

        public static IServiceCollection AddBlazoredLocalStorageStreaming(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            services.TryAddScoped<IStorageProvider, BrowserStreamingStorageProvider>();
            AddServices(services, configure);
            return services;
        }

        private static void AddServices(IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            services.TryAddScoped<IJsonSerializer, SystemTextJsonSerializer>();
            services.TryAddScoped<ILocalStorageService, LocalStorageService>();
            services.TryAddScoped<ISyncLocalStorageService, LocalStorageService>();
            if (services.All(serviceDescriptor => serviceDescriptor.ServiceType != typeof(IConfigureOptions<LocalStorageOptions>)))
            {
                services.Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
            }
        }

        /// <summary>
        /// Registers the Blazored LocalStorage services as singletons. This should only be used in Blazor WebAssembly applications.
        /// Using this in Blazor Server applications will cause unexpected and potentially dangerous behaviour. 
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddBlazoredLocalStorageAsSingleton(this IServiceCollection services)
            => AddBlazoredLocalStorageAsSingleton(services, null);

        /// <summary>
        /// Registers the Blazored LocalStorage services as singletons. This should only be used in Blazor WebAssembly applications.
        /// Using this in Blazor Server applications will cause unexpected and potentially dangerous behaviour. 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddBlazoredLocalStorageAsSingleton(this IServiceCollection services, Action<LocalStorageOptions>? configure)
        {
            services.TryAddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
            services.TryAddSingleton<IStorageProvider, BrowserStorageProvider>();
            services.TryAddSingleton<ILocalStorageService, LocalStorageService>();
            services.TryAddSingleton<ISyncLocalStorageService, LocalStorageService>();
            if (services.All(serviceDescriptor => serviceDescriptor.ServiceType != typeof(IConfigureOptions<LocalStorageOptions>)))
            {
                services.Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
            }
            return services;
        }
    }
}
