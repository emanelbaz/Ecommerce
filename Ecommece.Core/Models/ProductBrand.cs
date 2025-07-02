using Ecommece.Core.Models;

namespace Ecommece.Core.Models
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}