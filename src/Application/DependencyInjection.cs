using Application.EventHandlers.CreateBillToPayEvent;
using Application.Feature.BillToPay.EditBillToPay;
using Application.Feature.BillToPay.PayBillToPay;
using Application.Feature.FixedInvoice.CreateFixedInvoice;
using Application.Feature.FixedInvoice.SearchFixedInvoice;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<ICreateBillToPayEventHandler, CreateBillToPayEventHandler>();
            services.AddSingleton<ICreateFixedInvoiceHandler, CreateFixedInvoiceHandler>();
            services.AddSingleton<ISearchFixedInvoiceHandler, SearchFixedInvoiceHandler>();
            services.AddSingleton<IEditBillToPayHandler, EditBillToPayHandler>();
            services.AddSingleton<IPayBillToPayHandler, PayBillToPayHandler>();

            return services;
        }
    }
}