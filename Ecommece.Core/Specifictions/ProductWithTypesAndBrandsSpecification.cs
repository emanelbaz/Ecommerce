using Ecommece.Core.Models;
using System;
using System.Linq.Expressions;

namespace Ecommece.Core.Specifictions
{
    public class ProductWithTypesAndBrandsSpecification : BaseSpecifiction<Product>
    {
        // Constructor للـ Pagination + Filtering + Sorting
        public ProductWithTypesAndBrandsSpecification(Pagination paginationParams, string lang = "en")
            : base(x =>
                (string.IsNullOrEmpty(paginationParams.Search) ||
                 x.Translations.Any(t => t.Language == lang && t.Name.ToLower().Contains(paginationParams.Search.ToLower()))) &&
                (!paginationParams.BrandId.HasValue || paginationParams.BrandId == 0 || x.ProductBrandId == paginationParams.BrandId) &&
                (!paginationParams.TypeId.HasValue || paginationParams.TypeId == 0 || x.ProductTypeId == paginationParams.TypeId)
            )
        {
            // Includes
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.Variants);       // فقط الـ Variants
            AddInclude(p => p.Translations);   // الترانسليشنز

            // Sorting
            switch (paginationParams.Sort)
            {
                case "priceAsc":
                    AddorderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddorderByDesc(p => p.Price);
                    break;
                default:
                    AddorderBy(p => p.Id); // Default sorting
                    break;
            }

            // Paging
            ApplayPaging((paginationParams.PageIndex - 1) * paginationParams.PageSize,
                        paginationParams.PageSize);
        }

        // Constructor للـ Count
        public ProductWithTypesAndBrandsSpecification(Pagination paginationParams, bool isForCount, string lang = "en")
            : base(x =>
                (string.IsNullOrEmpty(paginationParams.Search) ||
                 x.Translations.Any(t => t.Language == lang && t.Name.ToLower().Contains(paginationParams.Search.ToLower()))) &&
                (!paginationParams.BrandId.HasValue || x.ProductBrandId == paginationParams.BrandId) &&
                (!paginationParams.TypeId.HasValue || x.ProductTypeId == paginationParams.TypeId)
            )
        {
            AddInclude(p => p.Translations);
        }

        // Constructor لجلب منتج واحد بالـ Id
        public ProductWithTypesAndBrandsSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.Variants);
            AddInclude(p => p.Translations);
        }
    }
}
