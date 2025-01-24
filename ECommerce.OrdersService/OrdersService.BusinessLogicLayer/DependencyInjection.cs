using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.BusinessLogicLayer.Mappers;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using OrdersService.BusinessLogicLayer.Services;
using OrdersServiceClass = OrdersService.BusinessLogicLayer.Services.OrdersService;
using OrdersService.BusinessLogicLayer.Validators;

namespace OrdersService.BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration config)
        {
            services.AddValidatorsFromAssemblyContaining<AddOrderDTOValidator>();

            services.AddAutoMapper(typeof(AddOrderDTOToOrderMappingProfile).Assembly);

            services.AddScoped<IOrdersService, OrdersServiceClass>();
            services.AddScoped<IValidationService, ValidationService>();

            return services;
        }
    }
}
