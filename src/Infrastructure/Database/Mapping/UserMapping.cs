using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
            builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.PasswordHash).HasMaxLength(512);
            builder.Property(x => x.GoogleSub).HasMaxLength(64);
            builder.HasIndex(x => x.GoogleSub).IsUnique().HasFilter("[GoogleSub] IS NOT NULL");
            builder.Property(x => x.CreatedAt).HasColumnType("datetime2").IsRequired();
            builder.Property(x => x.TrialStartsAt).HasColumnType("datetime2").IsRequired();
            builder.Property(x => x.TrialEndsAt).HasColumnType("datetime2").IsRequired();
            builder.Property(x => x.SubscriptionStatus).HasMaxLength(20).IsRequired();
            builder.Property(x => x.QuickCaptureKeyHash).HasMaxLength(64);
            builder.HasIndex(x => x.QuickCaptureKeyHash).IsUnique().HasFilter("[QuickCaptureKeyHash] IS NOT NULL");
            builder.Property(x => x.QuickCaptureKeyCreatedAt).HasColumnType("datetime2");
        }
    }
}
