using AutoMapper;
using Ecommece.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace Ecommece.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Product → ProductResponse
            CreateMap<Product, ProductResponse>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand != null ? s.ProductBrand.Name : null))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType != null ? s.ProductType.Name : null))
                .ForMember(d => d.Name, o => o.MapFrom((src, dest, destMember, context) =>
                {
                    var lang = context.Items["lang"]?.ToString() ?? "en";
                    return src.Translations.FirstOrDefault(t => t.Language == lang)?.Name;
                }))
                .ForMember(d => d.Description, o => o.MapFrom((src, dest, destMember, context) =>
                {
                    var lang = context.Items["lang"]?.ToString() ?? "en";
                    return src.Translations.FirstOrDefault(t => t.Language == lang)?.Description;
                }))
                .ForMember(d => d.Variants, o => o.MapFrom(s => s.Variants));

            // ProductVariantRequest → ProductVariant
            CreateMap<ProductVariantRequest, ProductVariant>();

            // ProductVariant → ProductVariantResponse
            CreateMap<ProductVariant, ProductVariantResponse>()
                .ForMember(d => d.Color, o => o.MapFrom((src, dest, destMember, context) =>
                {
                    var lang = context.Items["lang"]?.ToString() ?? "en";
                    if (src.Color == null) return null;
                    return lang == "ar" ? src.Color.NameAr : src.Color.NameEn;
                }))
                .ForMember(d => d.Size, o => o.MapFrom(s => s.Size != null ? s.Size.Name : null));

            // ProductRequest → Product
            CreateMap<ProductRequest, Product>()
                .ForMember(d => d.Translations, o => o.MapFrom(s => new List<ProductTranslation>
                {
                    new ProductTranslation { Language = "en", Name = s.NameEn, Description = s.DescriptionEn },
                    new ProductTranslation { Language = "ar", Name = s.NameAr, Description = s.DescriptionAr }
                }))
                .ForMember(d => d.Variants, o => o.MapFrom(s => s.Variants));

            // OrderItem → OrderItemResponse
            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(d => d.ProductName, o => o.MapFrom((src, dest, destMember, context) =>
                {
                    var lang = context.Items["lang"]?.ToString() ?? "en";
                    if (src.ProductVariant?.Product?.Translations == null) return src.ProductName;
                    var translation = src.ProductVariant.Product.Translations.FirstOrDefault(t => t.Language == lang);
                    return translation?.Name ?? src.ProductName;
                }))
                
                .ForMember(d => d.Price, o => o.MapFrom(src => src.Price))
                .ForMember(d => d.Quantity, o => o.MapFrom(src => src.Quantity));
        }
    }
}
