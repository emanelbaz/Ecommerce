using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; } 
        public string BuyerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public Address ShippingAddress { get; set; }

        // ✅ بدل الكائن الكامل بخيار نصي مبدئيًا
        public string ShippingMethod { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public decimal Subtotal { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public string PaymentMethod { get; set; } // ✅ أضفناه

        // ✅ دالة المجموع النهائي
        public decimal GetTotal() => Subtotal;
    }

    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        PaymentFailed
    }

    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public string BuyerEmail { get; set; }
        public List<OrderItemRequest> Items { get; set; } = new();
        public Address ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingMethod { get; set; }
    }

    
}
