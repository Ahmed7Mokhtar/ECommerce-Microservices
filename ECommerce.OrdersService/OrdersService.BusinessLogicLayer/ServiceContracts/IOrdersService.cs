using MongoDB.Driver;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.DataAccessLayer.Entities;

namespace OrdersService.BusinessLogicLayer.ServiceContracts
{
    public interface IOrdersService
    {
        Task<List<OrderDTO>> GetAll();
        Task<List<OrderDTO>> GetOrdersByCondition(FilterDefinition<Order> filter);
        Task<OrderDTO?> GetOrderByCondition(FilterDefinition<Order> filter);
        Task<OrderDTO?> Add(AddOrderDTO order);
        Task<OrderDTO?> Update(UpdateOrderDTO order);
        Task<bool> Delete(Guid id);
    }
}
