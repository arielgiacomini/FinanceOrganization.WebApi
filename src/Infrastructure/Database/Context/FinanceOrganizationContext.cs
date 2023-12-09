using Domain.Entities;
using Infrastructure.Database.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database.Context
{
    public class FinanceOrganizationContext : DbContext
    {
        private const string SQL_SERVER_CONNECTION_STRING = "SqlServer";
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public FinanceOrganizationContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString(SQL_SERVER_CONNECTION_STRING);
        }

        /// <summary>
        /// Conta/Fatura fixa.
        /// </summary>
        public DbSet<FixedInvoice> FixedInvoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FixedInvoiceMapping());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}