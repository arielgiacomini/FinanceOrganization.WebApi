using Application.EventHandlers.CreateBillToPayEvent;
using Application.Feature.CreateFixedInvoice;
using Application.Feature.SearchFixedInvoice;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<ICreateBillToPayEventHandler, CreateBillToPayEventHandler>();
            services.AddSingleton<ICreateFixedInvoiceHandler, CreateFixedInvoiceHandler>();
            services.AddSingleton<ISearchFixedInvoiceHandler, SearchFixedInvoiceHandler>();

            return services;
        }
    }
}