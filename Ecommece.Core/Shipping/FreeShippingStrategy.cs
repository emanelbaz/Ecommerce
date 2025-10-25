using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public class FreeShippingStrategy : IShippingStrategy
    {
        public decimal CalculateShippingCost(decimal orderTotal) => 0m;
        public string GetDescription() => "Free Shipping";
    }
}
