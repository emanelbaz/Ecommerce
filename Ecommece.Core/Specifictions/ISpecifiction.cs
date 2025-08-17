using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Specifictions
{
    public interface ISpecifiction<T>
    {
        // شرط (فلترة)
        Expression<Func<T, bool>> Criteria { get; }

        // الـ Includes (العلاقات اللي هتتعمل لها eager loading)
        List<Expression<Func<T, object>>> Includes { get; }

        // ترتيب
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDesc { get; }

        // Paging
        int Take { get; }
        int Skip { get; }
        bool IspagingEnabled { get; }
    }


}
