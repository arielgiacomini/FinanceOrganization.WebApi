using Application.EventHandlers.WalletToPay;
using Application.Feature.CreateFixedInvoice;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IWalletToPayHandler, WalletToPayHandler>();
            services.AddSingleton<ICreateFixedInvoiceHandler, CreateFixedInvoiceHandler>();

            return services;
        }
    }
}