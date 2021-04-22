using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.LocalStorage;
using Blazored.LocalStorage.Testing;

namespace Bunit
{
    [ExcludeFromCodeCoverage]
    public static class BUnitLocalStorageTestExtensions
    {
        public static IStorageProvider AddBlazoredLocalStorage(this TestContextBase context)
            => AddBlazoredLocalStorage(context, null);

        public static IStorageProvider AddBlazoredLocalStorage(this TestContextBase context, Action<LocalStorageOptions> configure)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            

            context.Services
                .AddSingleton<IJsonSerializer, SystemTextJsonSerializer>()
                .AddSingleton<IStorageProvider, InMemoryStorageProvider>()
                .AddSingleton<ILocalStorageService, LocalStorageService>()
                .AddSingleton<ISyncLocalStorageService, LocalStorageService>()
                .Configure<LocalStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });

            return context.Services.GetService<IStorageProvider>();
        }
    }
}
