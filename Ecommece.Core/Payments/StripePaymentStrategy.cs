using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Payments
{
    public class StripePaymentStrategy : IPaymentStrategy
    {
        public async Task<bool> ProcessPayment(decimal amount, string currency = "USD")
        {
            // simulate Stripe API call
            await Task.Delay(500);
            Console.WriteLine($"Processed {amount} {currency} via Stripe.");
            return true;
        }
    }
}
