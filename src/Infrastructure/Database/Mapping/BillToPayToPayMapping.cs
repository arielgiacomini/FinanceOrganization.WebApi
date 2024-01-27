using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class BillToPayToPayMapping : IEntityTypeConfiguration<BillToPay>
    {
        public void Configure(EntityTypeBuilder<BillToPay> builder)
        {
            builder.ToTable("CONTA_PAGAR");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID_CONTA_PAGAR").IsRequired();
            builder.Property(x => x.IdFixedInvoice).HasColumnName("ID_CONTA_PAGAR_CADASTRO");
            builder.Property(x => x.Account).HasColumnName("DSC_CONTA");
            builder.Property(x => x.Name).HasColumnName("DSC_DESCRICAO");
            builder.Property(x => x.Category).HasColumnName("DSC_CATEGORIA");
            builder.Property(x => x.RegistrationType).HasColumnName("IND_TIPO_REGISTRO");
            builder.Property(x => x.Value).HasColumnName("VAL_VALOR");
            builder.Property(x => x.PurchaseDate).HasColumnName("DAT_COMPRA").HasColumnType("datetime").HasDefaultValue(null);
            builder.Property(x => x.DueDate).HasColumnName("DAT_VENCIMENTO").HasColumnType("datetime").HasDefaultValue(null);
            builder.Property(x => x.YearMonth).HasColumnName("IND_MES_ANO");
            builder.Property(x => x.Frequence).HasColumnName("IND_FREQUENCIA");
            builder.Property(x => x.PayDay).HasColumnName("DAT_PAGAMENTO");
            builder.Property(x => x.HasPay).HasColumnName("IND_PAGO");
            builder.Property(x => x.AdditionalMessage).HasColumnName("DSC_MENSAGEM_ADICIONAL");
            builder.Property(x => x.CreationDate).HasColumnName("DAT_CRIACAO_REGISTRO").HasColumnType("datetime").HasDefaultValue(null);
            builder.Property(x => x.LastChangeDate).HasColumnName("DAT_ULTIMA_ALTERACAO").HasColumnType("datetime").HasDefaultValue(null);
        }
    }
}