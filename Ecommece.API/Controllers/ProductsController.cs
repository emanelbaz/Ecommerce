
using Ecommece.Core.Interfaces;
using Ecommerce.Core.Models;
using Ecommerce.EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
       IProductRepository _repo;
        public ProductsController(IProductRepository repo) {
            _repo = repo;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> getProducts()
        {
            var products= await _repo.GetAllAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> getProduct(int id) {
            return await _repo.getProductAsync(id);
        }

    }
}
