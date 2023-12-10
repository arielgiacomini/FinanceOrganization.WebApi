using Domain.Interfaces;
using Infrastructure.Database.Context;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<FinanceOrganizationContext>();
            services.AddScoped<IFixedInvoiceRepository, FixedInvoiceRepository>();

            return services;
        }
    }
}