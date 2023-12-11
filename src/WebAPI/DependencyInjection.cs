using Infrastructure.BackgroundServices;

namespace WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<CreateBillToPayBackgroundServices>();

            return services;
        }
    }
}