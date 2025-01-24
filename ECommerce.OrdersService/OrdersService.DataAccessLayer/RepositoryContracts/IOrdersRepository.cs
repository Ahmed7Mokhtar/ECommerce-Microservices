using MongoDB.Driver;
using OrdersService.DataAccessLayer.Entities;

namespace OrdersService.DataAccessLayer.RepositoryContracts
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<IEnumerable<Order>> GetOrdersByCondition(FilterDefinition<Order> filter);
        Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter);
        Task<Order?> Add(Order order);
        Task<Order?> Update(Order order);
        Task<bool> Delete(Guid id);
    }
}
