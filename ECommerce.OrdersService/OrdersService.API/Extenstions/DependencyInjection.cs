using OrdersService.BusinessLogicLayer.HttpClients;
using OrdersService.BusinessLogicLayer.Policies;
using Polly;

namespace OrdersService.API.Extenstions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<UsersMicroserviceClient>(client =>
            {
                client.BaseAddress = new Uri($"http://{config["UsersMicroserviceName"]}:{config["UsersMicroservicePort"]}/");
            })
            .AddPolicyHandler(services.BuildServiceProvider().GetRequiredService<IUserserMicroservicePolicies>().GetCombinedPolicy());

            services.AddHttpClient<ProductsMicroserviceClient>(client =>
            {
                client.BaseAddress = new Uri($"http://{config["ProductsMicroserviceName"]}:{config["ProductsMicroservicePort"]}/");
            })
            .AddPolicyHandler(services.BuildServiceProvider().GetRequiredService<IProductsMicroservicePolicies>().GetCombinedPolicy());

            return services;
        }
    }
}
