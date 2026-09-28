using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Configurations
{
    public class InventoryTransactionConfiguration
    : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(
            EntityTypeBuilder<InventoryTransaction> builder)
        {
            builder.ToTable("InventoryTransactions");

            builder.HasKey(x => x.InventoryTransactionId);

            builder.Property(x => x.ReferenceNumber)
                .HasMaxLength(100);

            builder.Property(x => x.Remarks)
                .HasMaxLength(500);

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.PreviousQuantity)
                .IsRequired();

            builder.Property(x => x.NewQuantity)
                .IsRequired();

            builder.Property(x => x.TransactionType)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.ProductId);

            builder.HasIndex(x => x.CreatedAt);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.InventoryTransactions)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
