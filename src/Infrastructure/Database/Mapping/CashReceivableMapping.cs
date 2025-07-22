using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class CashReceivableMapping : IEntityTypeConfiguration<CashReceivable>
    {
        public void Configure(EntityTypeBuilder<CashReceivable> builder)
        {
            builder.ToTable("CONTA_RECEBER");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("ID_CONTA_RECEBER").IsRequired();
            builder.Property(x => x.IdCashReceivableRegistration).HasColumnName("ID_CONTA_RECEBER_CADASTRO");
            builder.Property(x => x.Name).HasColumnName("DSC_DESCRICAO");
            builder.Property(x => x.Account).HasColumnName("DSC_CONTA");
            builder.Property(x => x.Category).HasColumnName("DSC_CATEGORIA");
            builder.Property(x => x.RegistrationType).HasColumnName("IND_TIPO_REGISTRO");
            builder.Property(x => x.Value).HasColumnName("VAL_VALOR");
            builder.Property(x => x.ManipulatedValue).HasColumnName("VAL_VALOR_MANIPULADO");
            builder.Property(x => x.AgreementDate).HasColumnName("DAT_ACORDO").HasColumnType("datetime");
            builder.Property(x => x.DueDate).HasColumnName("DAT_VENCIMENTO").HasColumnType("datetime");
            builder.Property(x => x.HasReceived).HasColumnName("IND_RECEBIDO");
            builder.Property(x => x.DateReceived).HasColumnName("DAT_RECEBIMENTO");
            builder.Property(x => x.YearMonth).HasColumnName("IND_MES_ANO");
            builder.Property(x => x.Frequence).HasColumnName("IND_FREQUENCIA");
            builder.Property(x => x.AdditionalMessage).HasColumnName("DSC_MENSAGEM_ADICIONAL");
            builder.Property(x => x.Enabled).HasColumnName("IND_ATIVO");
            builder.Property(x => x.CreationDate).HasColumnName("DAT_CRIACAO_REGISTRO").HasColumnType("datetime");
            builder.Property(x => x.LastChangeDate).HasColumnName("DAT_ULTIMA_ALTERACAO");
        }
    }
}