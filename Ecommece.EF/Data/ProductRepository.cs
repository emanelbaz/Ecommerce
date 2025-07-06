using Ecommece.Core.Interfaces;
using Ecommece.Core.Models;
using Ecommece.EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.EF.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context _context;
        public ProductRepository(Context context) 
        { 
            _context = context;
        }
        

        public async Task<List<Product>> GetAllProductAsync()
        {
            return await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();
        }
        public async Task<Product> getProductAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id==id);
        }
        public async Task<List<ProductBrand>> GetAllProductBrandAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<List<ProductType>> GetAllProductTypeAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        
    }
}
