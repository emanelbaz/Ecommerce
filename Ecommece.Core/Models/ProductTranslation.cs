using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class ProductTranslation
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Language { get; set; } = "en"; 
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }
    }
}
