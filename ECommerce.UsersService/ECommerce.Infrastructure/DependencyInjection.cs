using ECommerce.Core.Repositories;
using ECommerce.Infrastructure.DbContext;
using ECommerce.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extension method to add Infrastructure services to the DI Container
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //TODO: Add services to IOC Container
        services.AddTransient<IUsersRepository, UsersRepository>();

        services.AddTransient<DapperDbContext>();

        return services;
    }
}
