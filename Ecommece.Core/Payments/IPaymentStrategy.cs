using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Payments
{
    public interface IPaymentStrategy
    {
        Task<bool> ProcessPayment(decimal amount, string currency = "USD");
    }
}
