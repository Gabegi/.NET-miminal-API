using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(e => new { e.OrderId, e.ProductId });

        builder.Property(e => e.UnitPrice)
            .HasPrecision(18, 2);

        builder.HasIndex(e => e.ProductId);
    }
}
