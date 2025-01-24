using AutoMapper;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.DataAccessLayer.Entities;

namespace OrdersService.BusinessLogicLayer.Mappers
{
    public class UpdateOrderDTOToOrderMappingProfile : Profile
    {
        public UpdateOrderDTOToOrderMappingProfile()
        {
            CreateMap<UpdateOrderDTO, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest._id, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.TotalBill = dest.OrderItems.Sum(m => m.TotalPrice);
                });
        }
    }

    public class UpdateOrderItemDTOToOrderMappingProfile : Profile
    {
        public UpdateOrderItemDTOToOrderMappingProfile()
        {
            CreateMap<UpdateOrderItemDTO, OrderItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.UnitPrice * src.Quantity))
                .ForMember(dest => dest._id, opt => opt.Ignore());
        }
    }
}
