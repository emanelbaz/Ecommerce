using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; } // S, M, L, XL ...
        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}
