using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Infrastructure.EntityCofigs;

public class BusinessOperatorConfiguration : IEntityTypeConfiguration<BusinessOperator>
{
    public void Configure(EntityTypeBuilder<BusinessOperator> builder)
    {
        builder.ToTable("business_operators");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.SystemManagerId).HasColumnName("system_manager_id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(e => e.Representative).HasColumnName("representative").HasMaxLength(150);
        builder.Property(e => e.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(e => e.Username).HasColumnName("username").HasMaxLength(50);
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(256);

        builder.OwnsOne(e => e.Address, a =>
        {
            a.Property(p => p.Street).HasColumnName("street").HasMaxLength(200).IsRequired();
            a.Property(p => p.Ward).HasColumnName("ward").HasMaxLength(100);
            a.Property(p => p.District).HasColumnName("district").HasMaxLength(100);
            a.Property(p => p.City).HasColumnName("city").HasMaxLength(100).IsRequired();
        });

        builder.OwnsOne(e => e.ContactInfo, c =>
        {
            c.Property(p => p.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20).IsRequired();
            c.Property(p => p.Email).HasColumnName("email").HasMaxLength(100);
        });

        builder.HasOne<SystemManager>()
               .WithMany()
               .HasForeignKey(e => e.SystemManagerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
