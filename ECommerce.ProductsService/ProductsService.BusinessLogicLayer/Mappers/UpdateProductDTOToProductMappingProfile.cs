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
    public class UpdateProductDTOToProductMappingProfile : Profile
    {
        public UpdateProductDTOToProductMappingProfile()
        {
            CreateMap<UpdateProductDTO, Product>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Category))
                .ForMember(dest => dest.UnitPrice, opts => opts.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.QuantityInStock, opts => opts.MapFrom(src => src.QuantityInStock));
        }
    }
}
