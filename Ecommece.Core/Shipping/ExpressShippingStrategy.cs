using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public class ExpressShippingStrategy : IShippingStrategy
    {
        public decimal CalculateShippingCost(decimal orderTotal) => 50m;
        public string GetDescription() => "Express Shipping (1-2 days)";
    }
}
