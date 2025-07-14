using Infrastructure.BackgroundServices;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<CreateBillToPayBackgroundServices>();
            services.AddHostedService<CreateCategoryBackgroundServices>();
            services.AddHostedService<CreateCashReceivableBackgroundServices>();

            return services;
        }
    }
}