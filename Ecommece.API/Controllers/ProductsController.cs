
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
            var totalItems = await _repo.CountAsync(new ProductWithTypesAndBrandsSpecification(new Pagination { PageSize = int.MaxValue }));

            var data = _mapper.Map<IReadOnlyList<ProductResponse>>(products);

            return Ok(new PagedResult<ProductResponse>(data, totalItems, paginationParams.PageIndex, paginationParams.PageSize));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> getProduct(int id)
        {
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
