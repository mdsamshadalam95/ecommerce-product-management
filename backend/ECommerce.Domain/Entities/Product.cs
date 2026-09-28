using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string SKU { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public decimal? CostPrice { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.Draft;

        public string? Brand { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        // Navigation properties
        public Category Category { get; set; } = null!;

        public Inventory? Inventory { get; set; }

        public ICollection<InventoryTransaction> InventoryTransactions { get; set; }
            = new List<InventoryTransaction>();
    }
}
