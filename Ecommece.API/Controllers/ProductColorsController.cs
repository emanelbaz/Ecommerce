using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommece.API.Controllers
{
    [ApiController]
    [Route("api/colors")]
    public class ProductColorsController : ControllerBase
    {
        private readonly Context _context;
        public ProductColorsController(Context context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Color>>> GetColors() =>
            Ok(await _context.Colors.ToListAsync());

        [HttpPost]
        public async Task<ActionResult<Color>> AddColor(Color color)
        {
            _context.Colors.Add(color);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetColors), new { id = color.Id }, color);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color == null) return NotFound();
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
