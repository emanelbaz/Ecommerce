using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public string SKU { get; set; }
        public decimal? Price { get; set; } // optional override price
        public int StockQuantity { get; set; }

        // Navigation
        public Product Product { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
    }
    public class ProductVariantRequest
    {
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; }
    }
    public class ProductVariantResponse
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
    }

}
