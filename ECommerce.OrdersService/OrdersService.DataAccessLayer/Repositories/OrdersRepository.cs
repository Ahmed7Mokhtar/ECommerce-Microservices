using MongoDB.Driver;
using OrdersService.DataAccessLayer.Entities;
using OrdersService.DataAccessLayer.RepositoryContracts;

namespace OrdersService.BusinessLogicLayer.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private const string COLLECTION_NAME = "orders";

        private readonly IMongoCollection<Order> _orders;
        public OrdersRepository(IMongoDatabase mongoDatabase)
        {
            _orders = mongoDatabase.GetCollection<Order>(COLLECTION_NAME);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return (await _orders.FindAsync(Builders<Order>.Filter.Empty)).ToList();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            return (await _orders.FindAsync(filter)).ToList();
        }

        public async Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter)
        {
            return (await _orders.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<Order?> Add(Order order)
        {
            order.OrderId = Guid.NewGuid();
            order._id = order.OrderId;

            foreach (var item in order.OrderItems)
            {
                order._id = Guid.NewGuid();
            }

            await _orders.InsertOneAsync(order);

            return order;
        }

        public async Task<Order?> Update(Order order)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(or => or.OrderId, order.OrderId);

            Order existingOrder = (await _orders.FindAsync(filter)).FirstOrDefault();

            if (existingOrder is null)
                return null;

            order._id = existingOrder._id;

            ReplaceOneResult replace = await _orders.ReplaceOneAsync(filter, order);

            return order;
        }
        public async Task<bool> Delete(Guid id)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(order =>  order.OrderId, id);

            Order order = (await _orders.FindAsync(filter)).FirstOrDefault();

            //if(order is null)
            //    return false;

            DeleteResult result = await _orders.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
