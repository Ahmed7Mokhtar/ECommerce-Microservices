using AutoMapper;
using OrdersService.BusinessLogicLayer.DTOs;

namespace OrdersService.BusinessLogicLayer.Mappers
{
    internal class ProductDTOToOrderItemDTOMappingProfile : Profile
    {
        public ProductDTOToOrderItemDTOMappingProfile()
        {
            CreateMap<ProductDTO, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
        }
    }
}
