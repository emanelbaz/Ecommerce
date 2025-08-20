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
    : base(x =>
        (!paginationParams.BrandId.HasValue || paginationParams.BrandId == 0 || x.ProductBrandId == paginationParams.BrandId) &&
        (!paginationParams.TypeId.HasValue || paginationParams.TypeId == 0 || x.ProductTypeId == paginationParams.TypeId)
    )
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
