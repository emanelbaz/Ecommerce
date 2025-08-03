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

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDesc { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IspagingEnabled { get; private set; }

        protected void AddInclude (Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);

        }
        protected void AddorderBy(Expression<Func<T, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;


        }
        protected void AddorderByDesc(Expression<Func<T, object>> OrderByExpression)
        {
            OrderByDesc = OrderByExpression;


        }
        protected void ApplayPaging(int skip,int take)
        {
            Skip = skip;
            Take = take;
            IspagingEnabled = true;


        }
    }
}
