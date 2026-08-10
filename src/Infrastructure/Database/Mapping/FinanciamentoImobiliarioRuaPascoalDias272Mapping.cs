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
    public class FinanciamentoImobiliarioRuaPascoalDias272Mapping : 
        IEntityTypeConfiguration<FinanciamentoImobiliarioRuaPascoalDias272>
    {
        public void Configure(EntityTypeBuilder<FinanciamentoImobiliarioRuaPascoalDias272> builder)
        {
            builder.ToTable("FinanciamentoImobiliarioRuaPascoalDias272").HasNoKey();
            builder.Property(x => x.Installment).HasColumnName("Parcela");
            builder.Property(x => x.DueDate).HasColumnName("Vencimento");
            builder.Property(x => x.Amortization).HasColumnName("Amortizacao");
            builder.Property(x => x.Fees).HasColumnName("Juros");
            builder.Property(x => x.CorrectionIndex).HasColumnName("IndiceCorrecao");
            builder.Property(x => x.InsuranceMIP).HasColumnName("SeguroMIP");
            builder.Property(x => x.InsuranceDFI).HasColumnName("SeguroDFI");
            builder.Property(x => x.InsuranceRES).HasColumnName("SeguroRES");
            builder.Property(x => x.Fine).HasColumnName("Multa");
            builder.Property(x => x.Mora).HasColumnName("Mora");
            builder.Property(x => x.TCA).HasColumnName("TCA");
            builder.Property(x => x.FinancialAdjustment).HasColumnName("AjusteFinanceiro");
            builder.Property(x => x.MonthlyFGTS).HasColumnName("FGTSMensal");
            builder.Property(x => x.BalanceCorrectionIndex).HasColumnName("IndiceCorrecaoSaldo");
            builder.Property(x => x.InstallmentAgreement).HasColumnName("AcordoParcelado");
            builder.Property(x => x.Situation).HasColumnName("Situacao");
            builder.Property(x => x.TotalInstallmentAmount).HasColumnName("ValorTotalParcela");
            builder.Property(x => x.OutstandingBalance).HasColumnName("SaldoDevedor");
        }
    }
}