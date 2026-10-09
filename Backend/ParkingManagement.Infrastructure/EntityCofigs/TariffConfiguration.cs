using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParkingManagement.Domain.Entities;

namespace ParkingManagement.Infrastructure.EntityCofigs;

public class TariffConfiguration : IEntityTypeConfiguration<Tariff>
{
    public void Configure(EntityTypeBuilder<Tariff> builder)
    {
        builder.ToTable("tariffs");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.ParkingLotId).HasColumnName("parking_lot_id").IsRequired();
        builder.Property(e => e.VehicleTypeId).HasColumnName("vehicle_type_id").IsRequired();
        builder.Property(e => e.PricingType).HasColumnName("pricing_type").HasConversion<int>().IsRequired();
        builder.Property(e => e.EffectiveDate).HasColumnName("effective_date").IsRequired();

        builder.OwnsOne(e => e.Price, p =>
        {
            p.Property(pr => pr.Amount).HasColumnName("price_amount").HasPrecision(18, 2).IsRequired();
            p.Property(pr => pr.Currency).HasColumnName("price_currency").HasMaxLength(10).IsRequired();
        });

        builder.HasOne(e => e.ParkingLot)
               .WithMany(pl => pl.Tariffs)
               .HasForeignKey(e => e.ParkingLotId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.VehicleType)
               .WithMany()
               .HasForeignKey(e => e.VehicleTypeId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
