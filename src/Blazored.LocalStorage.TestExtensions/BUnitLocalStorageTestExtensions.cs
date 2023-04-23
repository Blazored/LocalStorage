using System;
using System.Diagnostics.CodeAnalysis;
using Blazored.LocalStorage;
using Blazored.LocalStorage.JsonConverters;
using Blazored.LocalStorage.Serialization;
using Blazored.LocalStorage.StorageOptions;
using Blazored.LocalStorage.TestExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit
{
    [ExcludeFromCodeCoverage]
    public static class BUnitLocalStorageTestExtensions
    {
        public static ILocalStorageService AddBlazoredLocalStorage(this TestContextBase context)
            => AddBlazoredLocalStorage(context, null);

        public static ILocalStorageService AddBlazoredLocalStorage(this TestContextBase context, Action<LocalStorageOptions>? configure)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var localStorageOptions = new LocalStorageOptions();
            configure?.Invoke(localStorageOptions);
            localStorageOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());

            var localStorageService = new LocalStorageService(new InMemoryStorageProvider(), new SystemTextJsonSerializer(localStorageOptions));
            context.Services.AddSingleton<ILocalStorageService>(localStorageService);

            return localStorageService;
        }
    }
}
