
using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommece.API.Controllers
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
            var products= await _repo.GetAllProductAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> getProduct(int id) {
            return await _repo.getProductAsync(id);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> getProductBrands()
        {
            var products = await _repo.GetAllProductBrandAsync();
            return Ok(products);
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<ProductBrand>>> getProductTypes()
        {
            var products = await _repo.GetAllProductTypeAsync();
            return Ok(products);
        }
    }
}
