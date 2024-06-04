using Application.EventHandlers.CreateBillToPayEvent;
using Application.Feature.BillToPay.CreateBillToPay;
using Application.Feature.BillToPay.CreateCreditCardNFCMobileBillToPay;
using Application.Feature.BillToPay.DeleteBillToPay;
using Application.Feature.BillToPay.EditBillToPay;
using Application.Feature.BillToPay.PayBillToPay;
using Application.Feature.BillToPay.SearchBillToPay;
using Application.Feature.BillToPay.SearchMonthlyAverageAnalysis;
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
            services.AddScoped<ICreateBillToPayHandler, CreateBillToPayHandler>();
            services.AddScoped<ISearchFixedInvoiceHandler, SearchFixedInvoiceHandler>();
            services.AddScoped<IEditBillToPayHandler, EditBillToPayHandler>();
            services.AddScoped<IPayBillToPayHandler, PayBillToPayHandler>();
            services.AddScoped<ISearchBillToPayHandler, SearchBillToPayHandler>();
            services.AddScoped<IDeleteBillToPayHandler, DeleteBillToPayHandler>();
            services.AddScoped<ISearchMonthlyAverageAnalysisHandler, SearchMonthlyAverageAnalysisHandler>();
            services.AddScoped<ICreateCreditCardNFCMobileBillToPayHandler, CreateCreditCardNFCMobileBillToPayHandler>();

            return services;
        }
    }
}