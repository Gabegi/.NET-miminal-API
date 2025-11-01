using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(e => e.Username)
            .IsUnique();

        builder.HasIndex(e => e.Email)
            .IsUnique();

        // Many-to-many relationship with Role
        builder.HasMany(e => e.Roles)
            .WithMany(e => e.Users)
            .UsingEntity("UserRoles",
                l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade),
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade));
    }
}
