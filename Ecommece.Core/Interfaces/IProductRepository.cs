using Ecommece.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> getProductAsync(int id);
        Task<List<Product>> GetAllProductAsync();
        Task<List<ProductBrand>> GetAllProductBrandAsync();
        Task<List<ProductType>> GetAllProductTypeAsync();
    }
}
