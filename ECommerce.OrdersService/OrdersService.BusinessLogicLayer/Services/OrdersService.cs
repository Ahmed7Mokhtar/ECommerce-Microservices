using AutoMapper;
using MongoDB.Driver;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.BusinessLogicLayer.HttpClients;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using OrdersService.DataAccessLayer.Entities;
using OrdersService.DataAccessLayer.RepositoryContracts;

namespace OrdersService.BusinessLogicLayer.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly UsersMicroserviceClient _usersMicroserviceClient;
        private readonly ProductsMicroserviceClient _productsMicroserviceClient;

        public OrdersService(IOrdersRepository ordersRepository, IMapper mapper, IValidationService validationService, UsersMicroserviceClient usersMicroserviceClient, ProductsMicroserviceClient productsMicroserviceClient)
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
            _validationService = validationService;
            _usersMicroserviceClient = usersMicroserviceClient;
            _productsMicroserviceClient = productsMicroserviceClient;
        }

        public async Task<List<OrderDTO>> GetAll()
        {
            var orders = _mapper.Map<List<OrderDTO>>(await _ordersRepository.GetAll());

            foreach (var order in orders.Where(order => order != null))
            {
                foreach (var item in order.Items)
                {
                    var product = await _productsMicroserviceClient.GetById(item.ProductId);
                    _mapper.Map<ProductDTO, OrderItemDTO>(product, item);
                }

                var user = await _usersMicroserviceClient.GetById(order.UserId);
                _mapper.Map<UserDTO, OrderDTO>(user, order);
            }

            return orders;
        }

        public async Task<List<OrderDTO>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            var orders = _mapper.Map<List<OrderDTO>>(await _ordersRepository.GetOrdersByCondition(filter));

            foreach (var order in orders.Where(order => order != null))
            {
                foreach (var item in order.Items)
                {
                    var product = await _productsMicroserviceClient.GetById(item.ProductId);
                    _mapper.Map<ProductDTO, OrderItemDTO>(product, item);
                }

                var user = await _usersMicroserviceClient.GetById(order.UserId);
                _mapper.Map<UserDTO, OrderDTO>(user, order);
            }

            return orders;
        }

        public async Task<OrderDTO?> GetOrderByCondition(FilterDefinition<Order> filter)
        {
            var order = _mapper.Map<OrderDTO?>(await _ordersRepository.GetOrderByCondition(filter));

            foreach (var item in order?.Items)
            {
                var product = await _productsMicroserviceClient.GetById(item.ProductId);
                _mapper.Map<ProductDTO, OrderItemDTO>(product, item);
            }

            var user = await _usersMicroserviceClient.GetById(order.UserId);
            _mapper.Map<UserDTO, OrderDTO>(user, order);

            return order;
        }

        public async Task<OrderDTO?> Add(AddOrderDTO order)
        {
            ArgumentNullException.ThrowIfNull(order);

            await _validationService.ValidateAsync(order);
            foreach (var item in order.Items)
            {
                await _validationService.ValidateAsync(item);
            }

            UserDTO user = await _usersMicroserviceClient.GetById(order.UserId);

            List<ProductDTO> loadedProducts = [];
            foreach (var item in order.Items)
            {
                ProductDTO product = await _productsMicroserviceClient.GetById(item.ProductId);
                loadedProducts.Add(product);
            }

            var toBeAddedOrder = _mapper.Map<Order>(order);

            var addedOrder = await _ordersRepository.Add(toBeAddedOrder);

            var orderResponse = _mapper.Map<OrderDTO?>(addedOrder);

            foreach (var item in orderResponse?.Items)
            {
                var product = loadedProducts.FirstOrDefault(m => m.ProductId == item.ProductId);
                _mapper.Map<ProductDTO, OrderItemDTO>(product!, item);
            }

            _mapper.Map<UserDTO, OrderDTO>(user, orderResponse);

            return orderResponse;
        }

        public async Task<OrderDTO?> Update(UpdateOrderDTO order)
        {
            ArgumentNullException.ThrowIfNull(order);

            await _validationService.ValidateAsync(order);
            foreach (var item in order.Items)
            {
                await _validationService.ValidateAsync(item);
            }

            UserDTO user = await _usersMicroserviceClient.GetById(order.UserId);

            List<ProductDTO> loadedProducts = [];
            foreach (var item in order.Items)
            {
                ProductDTO product = await _productsMicroserviceClient.GetById(item.ProductId);
                loadedProducts.Add(product);
            }

            var toBeUpdatedOrder = _mapper.Map<Order>(order);

            var updatedOrder = await _ordersRepository.Update(toBeUpdatedOrder);
            var orderResponse = _mapper.Map<OrderDTO?>(updatedOrder);

            foreach (var item in orderResponse?.Items)
            {
                var product = loadedProducts.FirstOrDefault(m => m.ProductId == item.ProductId);
                _mapper.Map<ProductDTO, OrderItemDTO>(product!, item);
            }

            _mapper.Map<UserDTO, OrderDTO>(user, orderResponse);

            return orderResponse;
        }

        public async Task<bool> Delete(Guid id)
        {
            var filter = Builders<Order>.Filter.Eq(m => m.OrderId, id);
            var toBeDeletedOrder = await _ordersRepository.GetOrderByCondition(filter);
            if(toBeDeletedOrder is null)
                return false;

            return await _ordersRepository.Delete(id);
        }
    }
}
