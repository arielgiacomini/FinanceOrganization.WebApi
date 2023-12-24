﻿using Domain.Interfaces;
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
            services.AddDbContext<FinanceOrganizationContext>();
            services.AddScoped<IFixedInvoiceRepository, FixedInvoiceRepository>();
            services.AddScoped<IWalletToPayRepository, WalletToPayRepository>();

            return services;
        }
    }
}