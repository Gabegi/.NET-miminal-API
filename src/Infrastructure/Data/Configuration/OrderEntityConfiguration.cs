using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.OrderDate)
            .HasDefaultValueSql("GETUTCDATE()")
            .ValueGeneratedOnAdd();

        builder.HasMany(e => e.Items)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.CustomerId);
        builder.HasIndex(e => e.OrderDate);
    }
}
