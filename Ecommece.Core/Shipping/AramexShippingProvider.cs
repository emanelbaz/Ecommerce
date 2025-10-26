using Ecommece.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public class AramexShippingProvider : IShippingProvider
    {
        public string Name => "Aramex";

        public string ShipOrder(int orderId)
        {
            return $"ARX-{orderId}-{Guid.NewGuid().ToString().Substring(0, 6)}";
        }
    }
}
