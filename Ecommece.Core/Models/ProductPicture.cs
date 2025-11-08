using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class ProductPicture
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string PictureUrl { get; set; }
        public bool IsMain { get; set; }

        public Product Product { get; set; }
    }

}
