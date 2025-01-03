using Microsoft.Extensions.DependencyInjection;
using ProductsService.BusinessLogicLayer.Mappers;
using ProductsService.BusinessLogicLayer.ServiceContracts;
using ProductsServiceClass = ProductsService.BusinessLogicLayer.Services.ProductsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ProductsService.BusinessLogicLayer.Validators;

namespace ProductsService.BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<IProductsService, ProductsServiceClass>();

            services.AddAutoMapper(typeof(AddProductDTOToProductMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<AddProductDTOValidator>();

            return services;
        }
    }
}
