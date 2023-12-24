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
            services.AddScoped<ICreateBillToPayEventHandler, CreateBillToPayEventHandler>();
            services.AddScoped<ICreateFixedInvoiceHandler, CreateFixedInvoiceHandler>();
            services.AddScoped<ISearchFixedInvoiceHandler, SearchFixedInvoiceHandler>();
            services.AddScoped<IEditBillToPayHandler, EditBillToPayHandler>();
            services.AddScoped<IPayBillToPayHandler, PayBillToPayHandler>();

            return services;
        }
    }
}