using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Specifictions
{
    public class BaseSpecifiction<T> : ISpecifiction<T>
    {
        public BaseSpecifiction() { }
        public BaseSpecifiction(Expression <Func<T,bool>> criteria, List<Expression<Func<T, object>>> includes) 
        {
            Criteria = criteria;
            Includes = includes;


        }
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        protected void AddInclude (Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);

        }
    }
}
