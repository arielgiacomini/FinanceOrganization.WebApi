using Domain.Entities.Extern;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class MonthlyAverageAnalysisMapping : IEntityTypeConfiguration<MonthlyAverageAnalysis>
    {
        public void Configure(EntityTypeBuilder<MonthlyAverageAnalysis> builder)
        {
            builder.HasNoKey();
            builder.Property(x => x.Category).HasColumnName("CATEGORIA");
            builder.Property(x => x.QuantityTotal).HasColumnName("QUANTIDADE_REGISTROS");
            builder.Property(x => x.SumTotal).HasColumnName("SOMA_TOTAL");
            builder.Property(x => x.MonthQuantity).HasColumnName("QUANTIDADE_MESES");
            builder.Property(x => x.AvgPriceMonthly).HasColumnName("MEDIA_VALOR_MENSAL");
            builder.Property(x => x.AvgQuantityMonthly).HasColumnName("MEDIA_QUANTIDADE_MENSAL");
            builder.Property(x => x.FistDate).HasColumnName("PRIMEIRA");
            builder.Property(x => x.LastDate).HasColumnName("ULTIMA");
        }
    }
}