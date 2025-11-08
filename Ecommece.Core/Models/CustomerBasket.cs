using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!; // basket key (guid or user key)
        public List<BasketItem> Items { get; set; } = new();
        public decimal GetTotal() => Items.Sum(i => i.Price * i.Quantity);
    }

    // BasketItem.cs
    public class BasketItem
    {
        // Important: we store VariantId (ProductVariant) not ProductId
        public int VariantId { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
    }
}
