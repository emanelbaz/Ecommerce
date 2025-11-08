using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class Color
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }

        public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }

}
