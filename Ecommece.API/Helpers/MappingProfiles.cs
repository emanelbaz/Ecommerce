using AutoMapper;
using Ecommece.Core.Models;

namespace Ecommece.API.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles() {
            CreateMap<Product, ProductResponse>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name));
        }
    }
}
