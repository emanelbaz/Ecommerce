using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class ShippingOrder
    {
        public int OrderId { get; set; }
        public string ShippingProvider { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
    }
}
