using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("CATEGORIA");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("ID_CATEGORIA")
                .IsRequired();
            builder.Property(x => x.Name)
                .HasColumnName("DSC_DESCRICAO");
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