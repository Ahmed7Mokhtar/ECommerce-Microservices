using AutoMapper;
using OrdersService.BusinessLogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.Mappers
{
    public class UserDTOToOrderDTOMappingProfile : Profile
    {
        public UserDTOToOrderDTOMappingProfile()
        {
            CreateMap<UserDTO, OrderDTO>()
                .ForMember(dest => dest.UserPersonName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.Email));
        }
    }
}
