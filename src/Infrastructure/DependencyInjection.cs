using Infrastructure.Database.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<FinanceOrganizationContext>();

            return services;
        }
    }
}