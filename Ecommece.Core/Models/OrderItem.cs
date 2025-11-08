using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class OrderItem : BaseEntity
    {
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; }

        public int OrderId { get; set; }   // <-- Added FK
        public Order Order { get; set; }   // <-- Navigation

        public string? ProductName { get; set; }
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderItemRequest
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
  
        public class OrderItemResponse
        {
            public int ProductVariantId { get; set; }
            public string? ProductName { get; set; }
            public string? PictureUrl { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }
    


}
