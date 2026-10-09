using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Infrastructure.EntityCofigs;

public class ParkingSessionConfiguration : IEntityTypeConfiguration<ParkingSession>
{
    public void Configure(EntityTypeBuilder<ParkingSession> builder)
    {
        builder.ToTable("parking_sessions");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ParkingLotId).HasColumnName("parking_lot_id").IsRequired();
        builder.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id").IsRequired();
        builder.Property(e => e.CheckInTime).HasColumnName("check_in_time").IsRequired();
        builder.Property(e => e.CheckOutTime).HasColumnName("check_out_time");
        builder.Property(e => e.Status).HasColumnName("status").HasConversion<int>().IsRequired();

        builder.OwnsOne(e => e.LicensePlate, lp =>
        {
            lp.Property(p => p.ProvinceCode).HasColumnName("province_code").HasMaxLength(20).IsRequired();
            lp.Property(p => p.PlateNumber).HasColumnName("plate_number").HasMaxLength(50).IsRequired();
        });

        builder.OwnsOne(e => e.Fee, f =>
        {
            f.Property(p => p.Amount).HasColumnName("fee_amount").HasPrecision(18, 2).IsRequired();
            f.Property(p => p.Currency).HasColumnName("fee_currency").HasMaxLength(10).IsRequired();
        });

        builder.HasOne(e => e.ParkingLot)
               .WithMany()
               .HasForeignKey(e => e.ParkingLotId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.VehicleType)
               .WithMany()
               .HasForeignKey(e => e.VehicleTypeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
