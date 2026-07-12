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
using Application.Feature.BillToPayRegistration.DisableBillToPayRegistration;
using Application.Feature.BillToPayRegistration.RecordsAwaitingCompleteRegistration;
using Application.Feature.BillToPayRegistration.SearchBillToPayRegistration;
using Application.Feature.CashReceivable.AdjustCashReceivable;
using Application.Feature.CashReceivable.DeleteCashReceivable;
using Application.Feature.CashReceivable.EditCashReceivable;
using Application.Feature.CashReceivable.ReceiveCashReceivable;
using Application.Feature.CashReceivable.SearchCashReceivable;
using Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration;
using Application.Feature.CashReceivableRegistration.DisableCashReceivableRegistration;
using Application.Feature.Category.SearchCategory;
using Application.Feature.Date.SearchAllWithFilters;
using Application.Feature.Date.SearchMonthYear;
using Application.Feature.Payment.AdjustPayament;
using Application.Feature.RealEstateFinancing.SearchRealEstateFinancing;
using Application.Feature.Wallet.CreateWallet;
using Application.Feature.Wallet.EditWallet;
using Application.Feature.Wallet.SearchWallet;
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
            services.AddScoped<IEditCashReceivableHandler, EditCashReceivableHandler>();
            services.AddScoped<ISearchDateAllWithFiltersHandler, SearchDateAllWithFiltersHandler>();
            services.AddScoped<ISearchMonthYearHandler, SearchMonthYearHandler>();
            services.AddScoped<IDeleteCashReceivableHandler, DeleteCashReceivableHandler>();
            services.AddScoped<IDisableBillToPayRegistrationHandler, DisableBillToPayRegistrationHandler>();
            services.AddScoped<IDisableCashReceivableRegistrationHandler, DisableCashReceivableRegistrationHandler>();
            services.AddScoped<ICreateWalletHandler, CreateWalletHandler>();
            services.AddScoped<ISearchWalletHandler, SearchWalletHandler>();
            services.AddScoped<IEditWalletHandler, EditWalletHandler>();
            services.AddScoped<IReceiveCashReceivableHandler, ReceiveCashReceivableHandler>();
            services.AddScoped<ISearchRealEstateFinancingHandler, SearchRealEstateFinancingHandler>();

            return services;
        }
    }
}