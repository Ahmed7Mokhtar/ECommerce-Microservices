using AutoMapper;
using ProductsService.BusinessLogicLayer.DTOs;
using ProductsService.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.BusinessLogicLayer.Mappers
{
    public class ProductToProductResponseDTOMappingProfile : Profile
    {
        public ProductToProductResponseDTOMappingProfile()
        {
            CreateMap<Product, ProductResponseDTO>()
                .ForMember(dest => dest.ProductId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductName, opts => opts.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
                .ForMember(dest => dest.UnitPrice, opts => opts.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.QuantityInStock, opts => opts.MapFrom(src => src.QuantityInStock));
        }
    }
}
