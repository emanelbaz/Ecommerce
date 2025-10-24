
using AutoMapper;
using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.Core.Specifictions;
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
        IMapper _mapper;
        public ProductsController(IProductRepository repo,IMapper mapper) {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductResponse>>> getProducts()
        {
            var products= await _repo.GetAllProductAsync();
            return Ok( _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductResponse>>(products));
        }
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<ProductResponse>>> GetProductsPaging([FromQuery] Pagination paginationParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(paginationParams);

            var products = await _repo.GetAllProductsAsync(spec);
            //var totalItems = await _repo.CountAsync(new ProductWithTypesAndBrandsSpecification(new Pagination { PageSize = int.MaxValue }));
            var countSpec = new ProductWithTypesAndBrandsSpecification(paginationParams, true);

            var totalItems = await _repo.CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<ProductResponse>>(products);

            return Ok(new PagedResult<ProductResponse>(data, totalItems, paginationParams.PageIndex, paginationParams.PageSize));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> getProduct(int id)
        {
            var product = await _repo.getProductAsync(id);
            if (product == null) return NotFound();

            var mapped = _mapper.Map<ProductResponse>(product);
            return Ok(mapped);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> getProductBrands()
        {
            var products = await _repo.GetAllProductBrandAsync();
            return Ok(products);
        }
        [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> getProductTypes()
        {
            var types = await _repo.GetAllProductTypeAsync();
            return Ok(types);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponse>> AddProduct([FromBody] ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            var created = await _repo.AddProductAsync(product);
            var response = _mapper.Map<ProductResponse>(created);
            return CreatedAtAction(nameof(getProduct), new { id = response.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct(int id, [FromBody] ProductRequest request)
        {
            var existing = await _repo.getProductAsync(id);
            if (existing == null) return NotFound();

            _mapper.Map(request, existing);
            var updated = await _repo.UpdateProductAsync(existing);

            return Ok(_mapper.Map<ProductResponse>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _repo.DeleteProductAsync(id);
            return NoContent();
        }
    }
}
