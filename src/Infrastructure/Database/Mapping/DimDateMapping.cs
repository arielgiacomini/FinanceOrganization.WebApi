using Domain.Entities.Extern;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class DimDateMapping : IEntityTypeConfiguration<DimDate>
    {
        public void Configure(EntityTypeBuilder<DimDate> builder)
        {
            builder.ToTable("DimData");
            builder.HasKey(x => x.Date);
            builder.Property(x => x.Date)
                .HasColumnName("Data")
                .HasColumnType("datetime")
                .IsRequired();
            builder.Property(x => x.Year)
                .HasColumnName("Ano");
            builder.Property(x => x.Month)
                .HasColumnName("Mes");
            builder.Property(x => x.Day)
                .HasColumnName("Dia");
            builder.Property(x => x.MonthName)
                .HasColumnName("NomeMes");
            builder.Property(x => x.MonthYear)
                .HasColumnName("MesAno");
            builder.Property(x => x.Trimester)
                .HasColumnName("Trimestre");
            builder.Property(x => x.DayWeekName)
                .HasColumnName("NomeDiaSemana");
            builder.Property(x => x.DayWeek)
                .HasColumnName("DiaSemana");
            builder.Property(x => x.WeekYear)
                .HasColumnName("SemanaAno");
            builder.Property(x => x.DayYear)
                .HasColumnName("DiaAno");
            builder.Property(x => x.IsWeekend)
                .HasColumnName("FimDeSemana");
            builder.Property(x => x.IsHoliday)
                .HasColumnName("EhFeriado");
            builder.Property(x => x.HolidayName)
                .HasColumnName("NomeFeriado");
        }
    }
}