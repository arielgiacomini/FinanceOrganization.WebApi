using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class WalletMapping : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.ToTable("CARTEIRA");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("COD").IsRequired().ValueGeneratedOnAdd();
            builder.Property(w => w.WalletKey).HasColumnName("DSC_CHAVE_CARTEIRA").HasMaxLength(100);
            builder.Property(w => w.WalletValue).HasColumnName("DSC_VALOR_CARTEIRA");
            builder.Property(x => x.CreationDate).HasColumnName("DAT_CRIACAO_REGISTRO");
            builder.Property(x => x.LastChangeDate).HasColumnName("DAT_ALTERACAO_REGISTRO");
        }
    }
}