using ApplicationServices.Privat24;
using Data.Repositories.Privat24;
using Microsoft.Extensions.DependencyInjection;

namespace Privat24WebApp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.AddTransient(typeof(ICurrencyRateRepository), typeof(CurrencyRateRepository));
        }

        public static IServiceCollection RegisterAppService(this IServiceCollection services)
        {
            return services.AddTransient(typeof(ICurrencyRateApplicationService), typeof(CurrencyRateApplicationService));
        }
    }
}