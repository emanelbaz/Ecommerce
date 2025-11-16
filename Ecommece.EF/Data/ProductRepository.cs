using Ecommece.Core.Caching;
using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.Core.Specifictions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommece.EF.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context _context;
        private readonly ICacheService _cache;

        public ProductRepository(Context context, ICacheService cache)
        {
            _context = context;
            _cache = cache;
        }

        // جلب جميع المنتجات بدون Specification (Cache)
        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            const string cacheKey = "products_all";
            var cached = await _cache.GetAsync<IReadOnlyList<Product>>(cacheKey);
            if (cached != null)
                return cached;

            var products = await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Size)
                .Include(p => p.Translations)
                .ToListAsync();

            await _cache.SetAsync(cacheKey, products, TimeSpan.FromMinutes(5));
            return products;
        }

        // جلب منتج بالـ Id
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            string cacheKey = $"product_{id}";
            var cached = await _cache.GetAsync<Product>(cacheKey);
            if (cached != null) return cached;

            var product = await _context.Products
                .AsNoTracking()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Size)
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
                await _cache.SetAsync(cacheKey, product, TimeSpan.FromMinutes(10));

            return product;
        }

        public async Task<List<ProductBrand>> GetAllProductBrandAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<List<ProductType>> GetAllProductTypeAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        // جلب المنتجات باستخدام Specification
        public async Task<IReadOnlyList<Product>> GetAllProductsAsync(ISpecifiction<Product> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecifiction<Product> spec)
        {
            return await ApplySpecification(spec, ignorePaging: true).CountAsync();
        }

        public IQueryable<Product> ApplySpecification(ISpecifiction<Product> spec, bool ignorePaging = false)
        {
            var query = SpecificationEvaluator<Product>.GetQuery(_context.Products.AsQueryable(), spec);

            // ignore paging
            if (ignorePaging && spec.IspagingEnabled)
            {
                query = query.Skip(0).Take(int.MaxValue);
            }

            // Include ThenInclude لـ Variants
            query = query
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Color)
                .Include(p => p.Variants)
                    .ThenInclude(v => v.Size);

            return query;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            await _cache.RemoveAsync("products_all");
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
