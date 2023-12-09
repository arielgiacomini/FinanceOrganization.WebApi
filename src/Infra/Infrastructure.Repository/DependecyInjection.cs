using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureRepository(this IServiceCollection services)
        {
            services.AddScoped<IFixedInvoiceRepository, FixedInvoiceRepository>();

            return services;
        }
    }
}