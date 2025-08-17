using Ecommece.Core.Models;
using Ecommece.Core.Specifictions;
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
        Task<IReadOnlyList<Product>> GetAllProductsAsync(ISpecifiction<Product> spec);
        Task<int> CountAsync(ISpecifiction<Product> spec);
        IQueryable<Product> ApplySpecification(ISpecifiction<Product> spec, bool ignorePaging = false);


    }
}
