using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class Payment : BaseEntity
    {
        public int OrderId { get; set; }
        public string PaymentIntentId { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
    }
}
