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
        /// Registro inicial de uma conta a pagar
        /// </summary>
        public DbSet<BillToPayRegistration>? BillToPayRegistration { get; set; }

        /// <summary>
        /// Conta a pagar
        /// </summary>
        public DbSet<BillToPay>? BillToPay { get; set; }

        /// <summary>
        /// Categoria
        /// </summary>
        public DbSet<Category>? Category { get; set; }

        /// <summary>
        /// Registro inicial de uma conta a receber
        /// </summary>
        public DbSet<CashReceivableRegistration>? CashReceivableRegistration { get; set; }

        /// <summary>
        /// Resultado da Stored Procedure: STP_CONTA_PAGAR_MEDIAS_MENSAIS
        /// </summary>
        public DbSet<MonthlyAverageAnalysis>? MonthlyAverageAnalysis { get; set; }

        /// <summary>
        /// Tabela de Contas
        /// </summary>
        public DbSet<Account>? Accounts { get; set; }

        /// <summary>
        /// Tabela de Cores de Contas
        /// </summary>
        public DbSet<AccountColor> AccountColors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BillToPayRegistrationMapping());
            modelBuilder.ApplyConfiguration(new BillToPayToPayMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new CashReceivableRegistrationMapping());
            modelBuilder.ApplyConfiguration(new MonthlyAverageAnalysisMapping());
            modelBuilder.ApplyConfiguration(new AccountMapping());
            modelBuilder.ApplyConfiguration(new AccountColorMapping());

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