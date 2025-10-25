using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Payments
{
    public class CashOnDeliveryStrategy : IPaymentStrategy
    {
        public async Task<bool> ProcessPayment(decimal amount, string currency = "USD")
        {
            await Task.Delay(100);
            Console.WriteLine($"Cash on delivery: {amount} {currency}");
            return true;
        }
    }
}
