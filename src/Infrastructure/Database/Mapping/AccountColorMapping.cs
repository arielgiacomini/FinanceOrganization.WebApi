using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class AccountColorMapping : IEntityTypeConfiguration<AccountColor>
    {
        public void Configure(EntityTypeBuilder<AccountColor> builder)
        {
            builder.ToTable("CONTA_COLOR");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ID_CONTA_COLOR")
                .IsRequired();
            builder.Property(x => x.AccountId)
                .HasColumnName("ID_CONTA");
            builder.Property(x => x.BackgroundColorHexadecimal)
                .HasColumnName("FUNDO_COLOR_HEX");
            builder.Property(x => x.FonteColorHexadecimal)
                .HasColumnName("FONTE_COLOR_HEX");
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