using AutoMapper;
using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.Core.Specifictions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommece.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // GET: api/products
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductResponse>>> GetProducts([FromQuery] string lang = "en")
        {
            var paginationParams = new Pagination { PageIndex = 1, PageSize = int.MaxValue };
            var spec = new ProductWithTypesAndBrandsSpecification(paginationParams);

            var products = await _repo.GetAllProductsAsync(spec);

            var mapped = _mapper.Map<IReadOnlyList<ProductResponse>>(products, opt =>
            {
                opt.Items["lang"] = lang;
            });

            return Ok(mapped);
        }

        // GET: api/products/paged
        [AllowAnonymous]
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetProductsPaging([FromQuery] Pagination paginationParams, [FromQuery] string lang = "en")
        {
            var spec = new ProductWithTypesAndBrandsSpecification(paginationParams);
            var products = await _repo.GetAllProductsAsync(spec);

            var countSpec = new ProductWithTypesAndBrandsSpecification(paginationParams, true);
            var totalItems = await _repo.CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<ProductResponse>>(products, opt =>
            {
                opt.Items["lang"] = lang;
            });

            return Ok(new PagedResult<ProductResponse>(data, totalItems, paginationParams.PageIndex, paginationParams.PageSize));
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id, [FromQuery] string lang = "en")
        {
            var product = await _repo.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var mapped = _mapper.Map<ProductResponse>(product, opt =>
            {
                opt.Items["lang"] = lang;
            });

            return Ok(mapped);
        }

        // GET: api/products/brands
        [HttpGet("brands")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            var brands = await _repo.GetAllProductBrandAsync();
            return Ok(brands);
        }

        // GET: api/products/types
        [HttpGet("types")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            var types = await _repo.GetAllProductTypeAsync();
            return Ok(types);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> AddProduct([FromBody] ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            var created = await _repo.AddProductAsync(product);

            var response = _mapper.Map<ProductResponse>(created);
            return CreatedAtAction(nameof(GetProduct), new { id = response.Id }, response);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct(int id, [FromBody] ProductRequest request)
        {
            var existing = await _repo.GetProductByIdAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(request, existing);
            var updated = await _repo.UpdateProductAsync(existing);

            var response = _mapper.Map<ProductResponse>(updated);
            return Ok(response);
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _repo.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
