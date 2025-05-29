
using DataAccess.Context;
using DataAccess.Interfaces;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.DependencyInjection;
using Resources;

namespace WebApi.Configurations
{
    public static class DependencyInjectionConfig
    {

        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {

            //Infrastructure
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Repositories
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();


            //Services





            //services.AddTransient<IStorageResolverService, StorageResolverService>();
            //Service With HttpClient            

            //Resources
            services.AddTransient<CommonResource>();
            services.AddTransient<ErrorResource>();

            return services;
        }
    }
}
