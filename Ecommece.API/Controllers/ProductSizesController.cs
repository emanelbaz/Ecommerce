using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Ecommece.API.Controllers
{
    [ApiController]
    [Route("api/sizes")]
    public class ProductSizesController : ControllerBase
    {
        private readonly Context _context;
        public ProductSizesController(Context context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Size>>> GetSizes() =>
            Ok(await _context.Sizes.ToListAsync());

        [HttpPost]
        public async Task<ActionResult<Size>> AddSize(Size size)
        {
            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSizes), new { id = size.Id }, size);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null) return NotFound();
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
