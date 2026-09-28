using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Enums
{
    public enum InventoryTransactionType
    {
        OpeningStock = 1,
        Purchase = 2,
        Sale = 3,
        Return = 4,
        Damage = 5,
        Adjustment = 6
    }
}
