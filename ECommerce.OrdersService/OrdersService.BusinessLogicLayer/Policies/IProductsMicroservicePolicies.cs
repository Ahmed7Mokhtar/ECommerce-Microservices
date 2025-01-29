using Polly;

namespace OrdersService.BusinessLogicLayer.Policies
{
    public interface IProductsMicroservicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
    }
}
