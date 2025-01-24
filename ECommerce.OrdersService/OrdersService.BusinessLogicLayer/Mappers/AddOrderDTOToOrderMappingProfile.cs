using AutoMapper;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.DataAccessLayer.Entities;

namespace OrdersService.BusinessLogicLayer.Mappers
{
    public class AddOrderDTOToOrderMappingProfile : Profile
    {
        public AddOrderDTOToOrderMappingProfile()
        {
            CreateMap<AddOrderDTO, Order>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest._id, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.TotalBill = dest.OrderItems.Sum(m => m.TotalPrice);
                });
        }
    }

    public class AddOrderItemDTOToOrderMappingProfile : Profile
    {
        public AddOrderItemDTOToOrderMappingProfile()
        {
            CreateMap<AddOrderItemDTO, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.UnitPrice * src.Quantity))
                .ForMember(dest => dest._id, opt => opt.Ignore());
        }
    }
}
