using Ecommece.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public class FedExShippingProvider : IShippingProvider
    {
        public string Name => "FedEx";

        public string ShipOrder(int orderId)
        {
            return $"FDX-{orderId}-{Guid.NewGuid().ToString().Substring(0, 6)}";
        }
    }
}
