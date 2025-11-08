using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommece.EF.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Context _context;

        public OrderRepository(Context context)
        {
            _context = context;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return _context.Database.BeginTransactionAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<List<ProductVariant>> GetVariantsByIdsAsync(List<int> variantIds)
        {
            if (variantIds == null || variantIds.Count == 0)
                return new List<ProductVariant>();

            // Use a loop with FindAsync to avoid CTE syntax issues
            // This is safe for small lists and avoids EF Core's CTE generation
            var variants = new List<ProductVariant>();
            foreach (var id in variantIds)
            {
                var variant = await _context.ProductVariants.FindAsync(id);
                if (variant != null)
                {
                    variants.Add(variant);
                }
            }

            if (variants.Count == 0)
                return variants;

            // Load related entities separately using FindAsync to avoid CTE issues
            var colorIds = variants.Select(v => v.ColorId).Distinct().ToList();
            var sizeIds = variants.Select(v => v.SizeId).Distinct().ToList();

            var colors = new List<Color>();
            foreach (var colorId in colorIds)
            {
                var color = await _context.Colors.FindAsync(colorId);
                if (color != null)
                    colors.Add(color);
            }

            var sizes = new List<Size>();
            foreach (var sizeId in sizeIds)
            {
                var size = await _context.Sizes.FindAsync(sizeId);
                if (size != null)
                    sizes.Add(size);
            }

            // Attach navigation properties manually
            foreach (var variant in variants)
            {
                variant.Color = colors.FirstOrDefault(c => c.Id == variant.ColorId);
                variant.Size = sizes.FirstOrDefault(s => s.Id == variant.SizeId);
            }

            return variants;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.ProductVariant)
                        .ThenInclude(v => v.Color)
                .Include(o => o.Items)
                    .ThenInclude(i => i.ProductVariant)
                        .ThenInclude(v => v.Size)
                .Include(o => o.ShippingAddress)
                .AsSplitQuery()
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.ShippingAddress)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
