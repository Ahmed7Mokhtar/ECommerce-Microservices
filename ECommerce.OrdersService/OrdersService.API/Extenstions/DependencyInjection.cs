using OrdersService.BusinessLogicLayer.HttpClients;

namespace OrdersService.API.Extenstions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<UsersMicroserviceClient>(client =>
            {
                client.BaseAddress = new Uri($"http://{config["UsersMicroserviceName"]}:{config["UsersMicroservicePort"]}/");
            });

            services.AddHttpClient<ProductsMicroserviceClient>(client =>
            {
                client.BaseAddress = new Uri($"http://{config["ProductsMicroserviceName"]}:{config["ProductsMicroservicePort"]}/"); 
            });

            return services;
        }
    }
}
