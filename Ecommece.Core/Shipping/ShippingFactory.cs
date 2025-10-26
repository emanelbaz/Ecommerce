using Ecommece.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public static class ShippingFactory
    {
        public static IShippingProvider Create(string provider)
        {
            return provider.ToLower() switch
            {
                "dhl" => new DHLShippingProvider(),
                "fedex" => new FedExShippingProvider(),
                "aramex" => new AramexShippingProvider(),
                _ => new DHLShippingProvider()
            };
        }
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
