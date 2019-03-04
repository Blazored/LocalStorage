using Microsoft.Extensions.DependencyInjection;

namespace Blazored.LocalStorage
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazoredLocalStorage(this IServiceCollection services)
        {
            return services.AddScoped<ILocalStorageService, LocalStorageService>();
        }
    }
}
