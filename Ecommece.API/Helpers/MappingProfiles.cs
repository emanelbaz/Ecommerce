using AutoMapper;
using Ecommece.Core.Models;

namespace Ecommece.API.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            // Product → ProductResponse
            CreateMap<Product, ProductResponse>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
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

            // ProductVariant → ProductVariantResponse
            CreateMap<ProductVariant, ProductVariantResponse>()
                .ForMember(d => d.Color, o => o.MapFrom((src, dest, destMember, context) =>
                {
                    var lang = context.Items["lang"]?.ToString() ?? "en";
                    return lang == "ar" ? src.Color.NameAr : src.Color.NameEn;
                }))
                .ForMember(d => d.Size, o => o.MapFrom(s => s.Size.Name));

            // ProductRequest → Product
            CreateMap<ProductRequest, Product>();
        }
    }
}
