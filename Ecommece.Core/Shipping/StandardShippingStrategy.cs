using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public class StandardShippingStrategy : IShippingStrategy
    {
        public decimal CalculateShippingCost(decimal orderTotal) => 20m;
        public string GetDescription() => "Standard Shipping (3-5 days)";
    }
}
