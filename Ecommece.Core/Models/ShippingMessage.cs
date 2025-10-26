using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class ShippingMessage
    {
        public int OrderId { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingMethod { get; set; }
    }
}
