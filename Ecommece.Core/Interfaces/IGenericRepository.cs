using Ecommece.Core.Models;
using Ecommece.Core.Specifictions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> getByIdAsync (int id);
        Task<IReadOnlyList<T>> listAllAsync();
        Task<T> getEntityWithSpec(ISpecifiction<T> spec);
        Task<IReadOnlyList<T>> listAllAsync(ISpecifiction<T> spec);


    }
}
