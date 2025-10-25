using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Shipping
{
    public interface IShippingStrategy
    {
        decimal CalculateShippingCost(decimal orderTotal);
        string GetDescription();
    }
}
