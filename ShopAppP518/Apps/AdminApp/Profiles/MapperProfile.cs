using AutoMapper;
using ShopAppP518.Apps.AdminApp.Dtos.CategoryDto;
using ShopAppP518.Apps.AdminApp.Dtos.ProductDto;
using ShopAppP518.Apps.AdminApp.Dtos.UserDto;
using ShopAppP518.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShopAppP518.Apps.AdminApp.Profiles
{
    public class MapperProfile : Profile
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public MapperProfile(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            var uriBuilder = new UriBuilder(_contextAccessor.HttpContext.Request.Scheme,
                _contextAccessor.HttpContext.Request.Host.Host
                , _contextAccessor.HttpContext.Request.Host.Port.Value);
            var url = uriBuilder.Uri.AbsoluteUri;

            CreateMap<Category, CategoryReturnDto>()
                .ForMember(dest => dest.ImageUrl, map => map.MapFrom(src => url + "img/" + src.ImageUrl));
            //.ForMember(dest => dest.ProductCount, map => map.MapFrom(src => src.Products.Count()));

            CreateMap<Product, ProductReturnDto>();
            CreateMap<AppUser, UserGetDto>();

        }
    }
}
