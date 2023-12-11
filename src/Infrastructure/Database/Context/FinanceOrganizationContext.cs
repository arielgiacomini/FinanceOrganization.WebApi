using Domain.Entities;
using Infrastructure.Database.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database.Context
{
    public class FinanceOrganizationContext : DbContext
    {
        private const string SQL_SERVER_CONNECTION_STRING = "SqlServer";
        private readonly string _connectionString;

        public FinanceOrganizationContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(SQL_SERVER_CONNECTION_STRING)!;
        }

        /// <summary>
        /// Conta/Fatura fixa.
        /// </summary>
        public DbSet<FixedInvoice>? FixedInvoice { get; set; }

        /// <summary>
        /// Conta a pagar
        /// </summary>
        public DbSet<BillToPay> BillToPay { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FixedInvoiceMapping());
            modelBuilder.ApplyConfiguration(new BillToPayToPayMapping());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}