using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Infrastructure.EntityCofigs;

public class SystemManagerConfiguration : IEntityTypeConfiguration<SystemManager>
{
    public void Configure(EntityTypeBuilder<SystemManager> builder)
    {
        builder.ToTable("system_managers");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(150).IsRequired();
        builder.Property(e => e.Position).HasColumnName("position").HasMaxLength(100);
        builder.Property(e => e.Username).HasColumnName("username").HasMaxLength(50).IsRequired();
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(256).IsRequired();
        builder.Property(e => e.Role).HasColumnName("role").HasConversion<int>().IsRequired();
        builder.Property(e => e.IsActive).HasColumnName("is_active").IsRequired();

        builder.OwnsOne(e => e.ContactInfo, c =>
        {
            c.Property(p => p.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20).IsRequired();
            c.Property(p => p.Email).HasColumnName("email").HasMaxLength(100);
        });
    }
}
