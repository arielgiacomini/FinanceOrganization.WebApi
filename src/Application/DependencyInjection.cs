using Application.Feature.EventHandlers.WalletToPay;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IWalletToPayHandler, WalletToPayHandler>();

            return services;
        }
    }
}