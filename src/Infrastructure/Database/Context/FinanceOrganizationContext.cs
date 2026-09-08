using Domain.Entities;
using Domain.Entities.Dashboard;
using Domain.Entities.Extern;
using Domain.Interfaces;
using Infrastructure.Database.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Database.Context
{
    public class FinanceOrganizationContext : DbContext
    {
        private const string SQL_SERVER_CONNECTION_STRING = "SqlServer";
        private readonly string _connectionString;
        private readonly ICurrentUserService _currentUserService;

        public FinanceOrganizationContext(IConfiguration configuration, ICurrentUserService currentUserService)
        {
            _connectionString = configuration.GetConnectionString(SQL_SERVER_CONNECTION_STRING)!;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Construtor usado em testes: recebe as options já configuradas (ex.: provider InMemory),
        /// para poder testar o filtro global de isolamento por usuário sem um SQL Server real.
        /// </summary>
        public FinanceOrganizationContext(DbContextOptions<FinanceOrganizationContext> options, ICurrentUserService currentUserService)
            : base(options)
        {
            _connectionString = string.Empty;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Registro inicial de uma conta a pagar
        /// </summary>
        public DbSet<BillToPayRegistration>? BillToPayRegistration { get; set; }

        /// <summary>
        /// Tabela de Contas a pagar
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

        /// <summary>
        /// Tabela de Contas a Receber
        /// </summary>
        public DbSet<CashReceivable> CashReceivable { get; set; }

        /// <summary>
        /// Tabela de Dimensão de Datas
        /// </summary>
        public DbSet<DimDate> DimDate { get; set; }

        /// <summary>
        /// Tabela de Carteiras
        /// </summary>
        public DbSet<Wallet> Wallets { get; set; }
        /// <summary>
        /// TAbela de detalhes sobre o financiamento do imovel
        /// </summary>
        public DbSet<FinanciamentoImobiliarioRuaPascoalDias263> FinanciamentoImobiliarioRuaPascoalDias263 { get; set; } //DbSet = acesso a uma tabela específica

        /// <summary>
        /// Tabela de Usuários (donos dos dados)
        /// </summary>
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BillToPayRegistrationMapping());
            modelBuilder.ApplyConfiguration(new BillToPayToPayMapping());
            modelBuilder.ApplyConfiguration(new CategoryMapping());
            modelBuilder.ApplyConfiguration(new CashReceivableRegistrationMapping());
            modelBuilder.ApplyConfiguration(new MonthlyAverageAnalysisMapping());
            modelBuilder.ApplyConfiguration(new AccountMapping());
            modelBuilder.ApplyConfiguration(new AccountColorMapping());
            modelBuilder.ApplyConfiguration(new CashReceivableMapping());
            modelBuilder.ApplyConfiguration(new DimDateMapping());
            modelBuilder.ApplyConfiguration(new WalletMapping());
            modelBuilder.ApplyConfiguration(new FinanciamentoImobiliarioRuaPascoalDias263Mapping());
            modelBuilder.ApplyConfiguration(new UserMapping());
       
            // Isolamento por usuário: único ponto de verdade. Toda leitura/escrita nessas
            // tabelas via EF Core passa por aqui automaticamente — nunca filtrar UserId
            // manualmente em repositório/handler. Ver ICurrentUserService.
            modelBuilder.Entity<Wallet>().HasQueryFilter(e => e.UserId == _currentUserService.UserId);
            modelBuilder.Entity<Account>().HasQueryFilter(e => e.UserId == _currentUserService.UserId);
            modelBuilder.Entity<Category>().HasQueryFilter(e => e.UserId == _currentUserService.UserId);
            modelBuilder.Entity<BillToPay>().HasQueryFilter(e => e.UserId == _currentUserService.UserId);
            modelBuilder.Entity<BillToPayRegistration>().HasQueryFilter(e => e.UserId == _currentUserService.UserId);
            modelBuilder.Entity<CashReceivable>().HasQueryFilter(e => e.UserId == _currentUserService.UserId);
            modelBuilder.Entity<CashReceivableRegistration>().HasQueryFilter(e => e.UserId == _currentUserService.UserId);
            modelBuilder.Entity<FinanciamentoImobiliarioRuaPascoalDias263>().HasQueryFilter(e => e.UserId == _currentUserService.UserId);

            modelBuilder.Entity<DailyGoalExpenseByCategoryDateDashboard>()
                .HasNoKey();

            modelBuilder.Entity<MonthlyCashFlowDashboard>()
                .HasNoKey();

            modelBuilder.Entity<DailyExpenseByCategoryAndAccountDateDashboard>()
                .HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }

            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            base.OnConfiguring(optionsBuilder);
        }
    }
}