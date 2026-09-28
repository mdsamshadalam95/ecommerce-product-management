using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        public int ProductId { get; set; }

        public int QuantityAvailable { get; set; }

        public int ReservedQuantity { get; set; }

        public int ReorderLevel { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
    }
}
