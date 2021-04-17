//using Blazored.LocalStorage.StorageOptions;
//using Bunit;
//using Bunit.TestDoubles;
//using System;

//namespace Blazored.LocalStorage.Testing
//{
//    public static class BUnitFakeLocalStorageExtensions
//    {
//        public static TestAuthorizationContext AddBlazoredLocalStorage(this TestContextBase context)
//        {
//            if (context is null)
//                throw new System.ArgumentNullException(nameof(context));

//            context.
            

//            //return services
//            //    .AddScoped<ILocalStorageService, LocalStorageService>()
//            //    .AddScoped<ISyncLocalStorageService, LocalStorageService>()
//            //    .Configure<LocalStorageOptions>(configureOptions =>
//            //    {
//            //        configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
//            //    });
//        }

//        public static TestAuthorizationContext AddBlazoredLocalStorage(this TestContextBase context, Action<LocalStorageOptions> configure)
//        {
//            if (context is null)
//                throw new System.ArgumentNullException(nameof(context));

//            //return services
//            //    .AddScoped<ILocalStorageService, LocalStorageService>()
//            //    .AddScoped<ISyncLocalStorageService, LocalStorageService>()
//            //    .Configure<LocalStorageOptions>(configureOptions =>
//            //    {
//            //        configure?.Invoke(configureOptions);
//            //        configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
//            //    });
//        }
//    }
//}
