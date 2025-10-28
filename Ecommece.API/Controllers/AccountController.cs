using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Ecommece.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly Context _context;
        private readonly ITokenService _tokenService;

        public AccountController(Context context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(x => x.Email == request.Email))
                return BadRequest("Email already exists");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserResponse
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
                return Unauthorized("Invalid email");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }

            return new UserResponse
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
