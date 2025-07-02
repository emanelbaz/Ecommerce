using Ecommece.Core.Models;

namespace Ecommece.Core.Models
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}