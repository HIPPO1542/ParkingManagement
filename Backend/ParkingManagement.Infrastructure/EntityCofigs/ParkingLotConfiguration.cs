using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Infrastructure.EntityCofigs;

public class ParkingLotConfiguration : IEntityTypeConfiguration<ParkingLot>
{
    public void Configure(EntityTypeBuilder<ParkingLot> builder)
    {
        builder.ToTable("parking_lots");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.BusinessOperatorId).HasColumnName("business_operator_id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        builder.Property(e => e.TotalCapacity).HasColumnName("total_capacity").IsRequired();
        builder.Property(e => e.AvailableSpots).HasColumnName("available_spots").IsRequired();
        builder.Property(e => e.OpenTime).HasColumnName("open_time").HasMaxLength(10);
        builder.Property(e => e.CloseTime).HasColumnName("close_time").HasMaxLength(10);
        builder.Property(e => e.IsActive).HasColumnName("is_active").IsRequired();

        builder.OwnsOne(e => e.Address, a =>
        {
            a.Property(p => p.Street).HasColumnName("street").HasMaxLength(200).IsRequired();
            a.Property(p => p.Ward).HasColumnName("ward").HasMaxLength(100);
            a.Property(p => p.District).HasColumnName("district").HasMaxLength(100);
            a.Property(p => p.City).HasColumnName("city").HasMaxLength(100).IsRequired();
        });

        builder.HasOne(e => e.BusinessOperator)
               .WithMany(o => o.ParkingLots)
               .HasForeignKey(e => e.BusinessOperatorId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
