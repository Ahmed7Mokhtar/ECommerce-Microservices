using AutoMapper;
using ECommerce.Core.DTOs;
using ECommerce.Core.Entities;

namespace ECommerce.Core.Mappers
{
    public class AppUserToUserDTOMappingProfile : Profile
    {
        public AppUserToUserDTOMappingProfile()
        {
            CreateMap<AppUser, UserDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender));
        }
    }
}
