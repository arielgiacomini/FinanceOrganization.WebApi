using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Mapping
{
    public class FinanciamentoImobiliarioRuaPascoalDias263Mapping : IEntityTypeConfiguration<FinanciamentoImobiliarioRuaPascoalDias263>
    {
        public void Configure(EntityTypeBuilder<FinanciamentoImobiliarioRuaPascoalDias263> builder)
        {
            builder.ToTable("FinanciamentoImobiliarioRuaPascoalDias263").HasNoKey();
            builder.Property(x => x.Installment).HasColumnName("Parcela");
            builder.Property(x => x.Serie).HasColumnName("Serie");
            builder.Property(x => x.CurrencyType).HasColumnName("Moeda");
            builder.Property(x => x.DueDate).HasColumnName("DataVencimento");
            builder.Property(x => x.AmortizationSystem).HasColumnName("SistemaAmort");
            builder.Property(x => x.EffectiveAnnualRateAA).HasColumnName("TaxaEfetivaAA");
            builder.Property(x => x.EffectiveAnnualRateAM).HasColumnName("TaxaEfetivaAM");
            builder.Property(x => x.NominalRateAA).HasColumnName("TaxaNominalAA");
            builder.Property(x => x.NominalRateAM).HasColumnName("TaxaNominalAM");
            builder.Property(x => x.Amortization).HasColumnName("Amortizacao");
            builder.Property(x => x.Fees).HasColumnName("Juros");
            builder.Property(x => x.InstallmentAdjustmentIndex).HasColumnName("IndiceCorrecaoParcela");
            builder.Property(x => x.SafeMIP).HasColumnName("SeguroMIP");
            builder.Property(x => x.SafeDFI).HasColumnName("SeguroDFI");
            builder.Property(x => x.TCA).HasColumnName("TCA");
            builder.Property(x => x.More).HasColumnName("Multa");
            builder.Property(x => x.Mora).HasColumnName("Mora");
            builder.Property(x => x.FinancialAdjustment).HasColumnName("AjusteFinanceiro");
            builder.Property(x => x.MonthlyFGTS).HasColumnName("FGTSMensal");
            builder.Property(x => x.BalanceAdjustmentIndex).HasColumnName("IndiceCorrecaoSaldo");
            builder.Property(x => x.OutstandingBalance).HasColumnName("SaldoDevedor");
            builder.Property(x => x.ResInsurance).HasColumnName("SeguroRES");
            builder.Property(x => x.Situation).HasColumnName("Situacao");
            builder.Property(x => x.TotalInstallmentAmount).HasColumnName("ValorTotalParcela");
        }
    }
}