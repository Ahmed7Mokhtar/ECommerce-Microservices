using Polly;

namespace OrdersService.BusinessLogicLayer.Policies
{
    public interface IUserserMicroservicePolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
    }
}
