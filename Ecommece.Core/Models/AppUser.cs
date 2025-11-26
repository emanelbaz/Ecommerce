using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Models
{
    public class AppUser
    {
        
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string DisplayName { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; } = "User"; // Default role
    }

    public class LoginRequest
    {
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string DisplayName { get; set; }
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }

    public class UserResponse
    {
        public string DisplayName { get; set; } 

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
