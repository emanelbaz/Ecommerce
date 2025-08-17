using Ecommece.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommece.Core.Specifictions
{
    public class ProductWithTypesAndBrandsSpecification : BaseSpecifiction<Product>
    {
        public ProductWithTypesAndBrandsSpecification(Pagination paginationParams)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            AddorderBy(p => p.Name);

            ApplayPaging(
                (paginationParams.PageIndex - 1) * paginationParams.PageSize,
                paginationParams.PageSize
            );
        }
    }
}
