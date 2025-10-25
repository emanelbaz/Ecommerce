using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public static class ShippingFactory
    {
        public static IShippingStrategy GetShippingStrategy(string method)
        {
            return method switch
            {
                "Standard" => new StandardShippingStrategy(),
                "Express" => new ExpressShippingStrategy(),
                "Free" => new FreeShippingStrategy(),
                _ => new StandardShippingStrategy()
            };
        }
    }
}
