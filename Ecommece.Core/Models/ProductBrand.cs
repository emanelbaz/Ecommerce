using Ecommece.Core.Models;
using System.Text.Json.Serialization;

namespace Ecommece.Core.Models
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}