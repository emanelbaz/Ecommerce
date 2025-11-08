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
        public DateTime OrderDate { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingMethod { get; set; }

        public Address ShippingAddress { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        PaymentFailed
    }

    
    public class CreateOrderRequest
    {
        public string BasketId { get; set; }
        public int UserId { get; set; }
        public string BuyerEmail { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingMethod { get; set; }
        public Address ShippingAddress { get; set; }
    }
}
