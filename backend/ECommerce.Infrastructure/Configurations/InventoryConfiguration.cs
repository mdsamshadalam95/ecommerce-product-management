using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable("Inventory");

            builder.HasKey(x => x.InventoryId);

            builder.Property(x => x.QuantityAvailable)
                .IsRequired();

            builder.Property(x => x.ReservedQuantity)
                .IsRequired();

            builder.Property(x => x.ReorderLevel)
                .IsRequired();

            builder.Property(x => x.LastUpdatedAt)
                .IsRequired();

            builder.HasIndex(x => x.ProductId)
                .IsUnique();

            builder.HasOne(x => x.Product)
                .WithOne(x => x.Inventory)
                .HasForeignKey<Inventory>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
