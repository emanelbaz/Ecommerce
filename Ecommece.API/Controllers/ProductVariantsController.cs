using AutoMapper;
using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommece.API.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/variants")]
    public class ProductVariantsController : ControllerBase
    {
        private readonly Context _context;
        private readonly IMapper _mapper;

        public ProductVariantsController(Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVariant>>> GetVariants(int productId)
        {
            var variants = await _context.ProductVariants
                .Include(v => v.Color)
                .Include(v => v.Size)
                .Where(v => v.ProductId == productId)
                .ToListAsync();

            return Ok(variants);
        }

        [HttpPost]
        public async Task<ActionResult<ProductVariant>> AddVariant(int productId, [FromBody] ProductVariantRequest variantRequest)
        {
            variantRequest.ProductId = productId;

            var variant = _mapper.Map<ProductVariant>(variantRequest);

            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVariants), new { productId }, variant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVariant(int productId, int id)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.ProductId == productId && v.Id == id);

            if (variant == null)
                return NotFound();

            _context.ProductVariants.Remove(variant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
