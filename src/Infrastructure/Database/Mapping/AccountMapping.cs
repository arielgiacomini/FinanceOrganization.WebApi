using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class AccountMapping : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("CONTA");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ID_CONTA")
                .IsRequired();
            builder.Property(x => x.Name)
                .HasColumnName("DSC_DESCRICAO");
            builder.Property(x => x.DueDate)
                .HasColumnName("IND_DIA_VENCIMENTO");
            builder.Property(x => x.ClosingDay)
                .HasColumnName("IND_DIA_FECHAMENTO");
            builder.Property(x => x.ConsiderPaid)
                .HasColumnName("IND_CONSIDERA_PAGO");
            builder.Property(x => x.AccountAgency)
                .HasColumnName("DSC_NUMERO_AGENCIA");
            builder.Property(x => x.AccountNumber)
                .HasColumnName("DSC_NUMERO_CONTA");
            builder.Property(x => x.AccountDigit)
                .HasColumnName("DSC_DIGITO_CONTA");
            builder.Property(x => x.CardNumber)
                .HasColumnName("DSC_QUATRO_ULTIMOS_DIGITOS_CARTAO");
            builder.Property(x => x.CommissionPercentage)
                .HasColumnName("VAL_PERCENTUAL_COMISSAO");
            builder.Property(x => x.Enable)
                .HasColumnName("IND_ATIVO");
            builder.Property(x => x.CreationDate)
                .HasColumnName("DAT_CRIACAO_REGISTRO")
                .HasColumnType("datetime");
            builder.Property(x => x.LastChangeDate)
                .HasColumnName("DAT_ULTIMA_ALTERACAO")
                .HasColumnType("datetime")
                .HasDefaultValue(null);
        }
    }
}