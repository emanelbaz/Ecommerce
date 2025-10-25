using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Payments
{
    public static class PaymentStrategyFactory
    {
        public static IPaymentStrategy GetPaymentStrategy(string method)
        {
            return method switch
            {
                "Stripe" => new StripePaymentStrategy(),
                "PayPal" => new PayPalPaymentStrategy(),
                "Cash" => new CashOnDeliveryStrategy(),
                _ => new CashOnDeliveryStrategy()
            };
        }
    }
}
