using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class InventoryTransaction
    {
        public long InventoryTransactionId { get; set; }

        public int ProductId { get; set; }

        public InventoryTransactionType TransactionType { get; set; }

        public int Quantity { get; set; }

        public int PreviousQuantity { get; set; }

        public int NewQuantity { get; set; }

        public string? ReferenceNumber { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        // Navigation property
        public Product Product { get; set; } = null!;
    }
}
