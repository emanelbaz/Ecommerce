using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class Address
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required, MaxLength(100)]
        public string Street { get; set; }
        [Required, MaxLength(100)]
        public string City { get; set; }
        [Required, MaxLength(100)]
        public string Country { get; set; }
    }
}
