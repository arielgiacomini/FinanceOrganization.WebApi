using Domain.Entities;
using Domain.Entities.Extern;
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
        public DbSet<BillToPay>? BillToPay { get; set; }

        /// <summary>
        /// Categoria
        /// </summary>
        public DbSet<Category>? Category { get; set; }

        /// <summary>
        /// Resultado da Stored Procedure: STP_CONTA_PAGAR_MEDIAS_MENSAIS
        /// </summary>
        public DbSet<MonthlyAverageAnalysis>? MonthlyAverageAnalysis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FixedInvoiceMapping());
            modelBuilder.ApplyConfiguration(new BillToPayToPayMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new MonthlyAverageAnalysisMapping());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }
    }
}