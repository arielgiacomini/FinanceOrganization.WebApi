using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class FixedInvoiceMapping : IEntityTypeConfiguration<FixedInvoice>
    {
        public void Configure(EntityTypeBuilder<FixedInvoice> builder)
        {
            builder.ToTable("CONTA_PAGAR_CADASTRO");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID_CONTA_PAGAR_CADASTRO").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnName("DSC_DESCRICAO");
            builder.Property(x => x.Account).HasColumnName("DSC_CONTA");
            builder.Property(x => x.Category).HasColumnName("DSC_CATEGORIA");
            builder.Property(x => x.RegistrationType).HasColumnName("IND_TIPO_REGISTRO");
            builder.Property(x => x.Value).HasColumnName("VAL_VALOR");
            builder.Property(x => x.PurchaseDate).HasColumnName("DAT_COMPRA").HasColumnType("datetime");
            builder.Property(x => x.BestPayDay).HasColumnName("IND_DIA_PARA_PAGAMENTO");
            builder.Property(x => x.InitialMonthYear).HasColumnName("IND_MES_ANO_INICIAL");
            builder.Property(x => x.FynallyMonthYear).HasColumnName("IND_MES_ANO_FINAL");
            builder.Property(x => x.Frequence).HasColumnName("IND_FREQUENCIA");
            builder.Property(x => x.AdditionalMessage).HasColumnName("DSC_MENSAGEM_ADICIONAL");
            builder.Property(x => x.Enabled).HasColumnName("IND_ATIVO");
            builder.Property(x => x.CreationDate).HasColumnName("DAT_CRIACAO_REGISTRO");
            builder.Property(x => x.LastChangeDate).HasColumnName("DAT_ULTIMA_ALTERACAO");
        }
    }
}