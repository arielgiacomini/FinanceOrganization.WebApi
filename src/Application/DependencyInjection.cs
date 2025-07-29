using Application.EventHandlers.CreateBillToPayEvent;
using Application.EventHandlers.CreateCashReceivableEvent;
using Application.EventHandlers.CreateCategoryEvent;
using Application.Feature.Account.CreateAccount;
using Application.Feature.Account.SearchAccount;
using Application.Feature.Account.SearchAccountOnlyName;
using Application.Feature.BillToPay.DeleteBillToPay;
using Application.Feature.BillToPay.EditBillToPay;
using Application.Feature.BillToPay.PayBillToPay;
using Application.Feature.BillToPay.SearchBillToPay;
using Application.Feature.BillToPay.SearchMonthlyAverageAnalysis;
using Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;
using Application.Feature.BillToPayRegistration.CreateCreditCardNFCMobileBillToPayRegistration;
using Application.Feature.BillToPayRegistration.RecordsAwaitingCompleteRegistration;
using Application.Feature.BillToPayRegistration.SearchBillToPayRegistration;
using Application.Feature.CashReceivable.AdjustCashReceivable;
using Application.Feature.CashReceivable.SearchCashReceivable;
using Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration;
using Application.Feature.Category.SearchCategory;
using Application.Feature.Payment.AdjustPayament;
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
            services.AddScoped<ICreateNFCMobileBillToPayRegistrationHandler, CreateNFCMobileBillToPayRegistrationHandler>();
            services.AddScoped<ISearchCategoryHandler, SearchCategoryHandler>();
            services.AddScoped<ICreateCategoryEventHandler, CreateCategoryEventHandler>();
            services.AddScoped<ICreateCashReceivableRegistrationHandler, CreateCashReceivableRegistrationHandler>();
            services.AddScoped<ISearchAccountOnlyNameHandler, SearchAccountOnlyNameHandler>();
            services.AddScoped<ISearchAccountHandler, SearchAccountHandler>();
            services.AddScoped<ICreateAccountHandler, CreateAccountHandler>();
            services.AddScoped<ICreateCashReceivableEventHandler, CreateCashReceivableEventHandler>();
            services.AddScoped<IRecordsAwaitingCompleteRegistrationHandler, RecordsAwaitingCompleteRegistrationHandler>();
            services.AddScoped<IAdjustCashReceivableHandler, AdjustCashReceivableHandler>();
            services.AddScoped<IPaymentAdjustmentHandler, PaymentAdjustmentHandler>();
            services.AddScoped<ISearchCashReceivableHandler, SearchCashReceivableHandler>();

            return services;
        }
    }
}