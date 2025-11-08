using Ecommece.Core.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<ProductVariant>> GetVariantsByIdsAsync(List<int> variantIds);
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task AddAsync(Order order);
        Task SaveChangesAsync();
        
    }
}
