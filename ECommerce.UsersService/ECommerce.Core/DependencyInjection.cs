using ECommerce.Core.ServiceContracts;
using ECommerce.Core.Services;
using ECommerce.Core.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Core;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add Core services to the DI Container
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        //TODO: Add services to IOC Container
        services.AddTransient<IUsersService, UsersService>();

        services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();

        return services;
    }
}
