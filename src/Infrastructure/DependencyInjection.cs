using Domain.Interfaces;
using Infrastructure.Database.Context;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<FinanceOrganizationContext>();
            services.AddScoped<IBillToPayRegistrationRepository, BillToPayRegistrationRepository>();
            services.AddScoped<IBillToPayRepository, BillToPayRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICashReceivableRegistrationRepository, CashReceivableRegistrationRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICashReceivableRepository, CashReceivableRepository>();

            return services;
        }
    }
}