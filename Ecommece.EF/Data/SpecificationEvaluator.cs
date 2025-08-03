using Ecommece.Core.Models;
using Ecommece.Core.Specifictions;
using Microsoft.EntityFrameworkCore;

namespace Ecommece.EF.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifiction<T> spec)
        {
            var query = inputQuery;

            // Apply where clause
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if (spec.IspagingEnabled )
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            // Apply includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}