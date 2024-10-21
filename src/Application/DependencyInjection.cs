using Application.EventHandlers.CreateBillToPayEvent;
using Application.EventHandlers.CreateCategoryEvent;
using Application.Feature.BillToPay.DeleteBillToPay;
using Application.Feature.BillToPay.EditBillToPay;
using Application.Feature.BillToPay.PayBillToPay;
using Application.Feature.BillToPay.SearchBillToPay;
using Application.Feature.BillToPay.SearchMonthlyAverageAnalysis;
using Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;
using Application.Feature.BillToPayRegistration.CreateCreditCardNFCMobileBillToPayRegistration;
using Application.Feature.BillToPayRegistration.SearchBillToPayRegistration;
using Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration;
using Application.Feature.Category.SearchCategory;
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
            services.AddScoped<ICreateBillToPayRegistrationHandler, CreateBillToPayRegistrationHandler>();
            services.AddScoped<ISearchBillToPayRegistrationHandler, SearchBillToPayRegistrationHandler>();
            services.AddScoped<IEditBillToPayHandler, EditBillToPayHandler>();
            services.AddScoped<IPayBillToPayHandler, PayBillToPayHandler>();
            services.AddScoped<ISearchBillToPayHandler, SearchBillToPayHandler>();
            services.AddScoped<IDeleteBillToPayHandler, DeleteBillToPayHandler>();
            services.AddScoped<ISearchMonthlyAverageAnalysisHandler, SearchMonthlyAverageAnalysisHandler>();
            services.AddScoped<ICreateCreditCardNFCMobileBillToPayRegistrationHandler, CreateCreditCardNFCMobileBillToPayRegistrationHandler>();
            services.AddScoped<ISearchCategoryHandler, SearchCategoryHandler>();
            services.AddScoped<ICreateCategoryEventHandler, CreateCategoryEventHandler>();
            services.AddScoped<ICreateCashReceivableRegistrationHandler, CreateCashReceivableRegistrationHandler>();

            return services;
        }
    }
}