using Ecommece.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public class DHLShippingProvider : IShippingProvider
    {
        public string Name => "DHL";

        public string ShipOrder(int orderId)
        {
            // simulate generating tracking number
            return $"DHL-{orderId}-{Guid.NewGuid().ToString().Substring(0, 6)}";
        }
    }
}
