using AutoMapper;
using FluentValidation;
using MongoDB.Driver;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using OrdersService.BusinessLogicLayer.Validators;
using OrdersService.DataAccessLayer.Entities;
using OrdersService.DataAccessLayer.RepositoryContracts;
using System;

namespace OrdersService.BusinessLogicLayer.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<AddOrderDTO> _addOrderValidator;
        private readonly IValidator<AddOrderItemDTO> _addOrderItemValidator;
        private readonly IValidator<UpdateOrderDTO> _updateOrderValidator;
        private readonly IValidator<UpdateOrderItemDTO> _updateOrderItemValidator;
        private readonly IValidationService _validationService;

        public OrdersService(IOrdersRepository ordersRepository, IMapper mapper,
            //IValidator<AddOrderDTO> addOrderValidator, IValidator<AddOrderItemDTO> addOrderItemValidator,
            //IValidator<UpdateOrderDTO> updateOrderValidator, IValidator<UpdateOrderItemDTO> updateOrderItemValidator, 
            IValidationService validationService
            )
        {
            _ordersRepository = ordersRepository;
            _mapper = mapper;
            //_addOrderValidator = addOrderValidator;
            //_addOrderItemValidator = addOrderItemValidator;
            //_updateOrderValidator = updateOrderValidator;
            //_updateOrderItemValidator = updateOrderItemValidator;
            _validationService = validationService;
        }

        public async Task<List<OrderDTO>> GetAll()
        {
            return _mapper.Map<List<OrderDTO>>(await _ordersRepository.GetAll());
        }
        public async Task<List<OrderDTO>> GetOrdersByCondition(FilterDefinition<Order> filter)
        {
            return _mapper.Map<List<OrderDTO>>(await _ordersRepository.GetOrdersByCondition(filter));
        }

        public async Task<OrderDTO?> GetOrderByCondition(FilterDefinition<Order> filter)
        {
            return _mapper.Map<OrderDTO?>(await _ordersRepository.GetOrderByCondition(filter));
        }

        public async Task<OrderDTO?> Add(AddOrderDTO order)
        {
            ArgumentNullException.ThrowIfNull(order);

            //var result = await _addOrderValidator.ValidateAsync(order);
            //if(!result.IsValid)
            //{
            //    string errors = string.Join(", ", result.Errors.Select(err => err.ErrorMessage));
            //    throw new ArgumentException(errors);
            //}

            //foreach (var item in order.Items)
            //{
            //    result = await _addOrderItemValidator.ValidateAsync(item);
            //    if(!result.IsValid)
            //    {
            //        string errors = string.Join(", ", result.Errors.Select(err => err.ErrorMessage));
            //        throw new ArgumentException(errors);
            //    }
            //}

            await _validationService.ValidateAsync(order);
            foreach (var item in order.Items)
            {
                await _validationService.ValidateAsync(item);
            }

            //TODO: Check UserID exists in users microservice

            var toBeAddedOrder = _mapper.Map<Order>(order);

            //var totalBill = 0m;
            //foreach (var item in toBeAddedOrder.OrderItems)
            //{
            //    item.TotalPrice = item.UnitPrice * item.Quantity;
            //    totalBill += item.TotalPrice;
            //}
            //toBeAddedOrder.TotalBill = totalBill;

            var addedOrder = await _ordersRepository.Add(toBeAddedOrder);

            return _mapper.Map<OrderDTO?>(addedOrder);
        }

        public async Task<OrderDTO?> Update(UpdateOrderDTO order)
        {
            ArgumentNullException.ThrowIfNull(order);

            await _validationService.ValidateAsync(order);
            foreach (var item in order.Items)
            {
                await _validationService.ValidateAsync(item);
            }

            //TODO: Check UserID exists in users microservice

            var toBeUpdatedOrder = _mapper.Map<Order>(order);

            var updatedOrder = await _ordersRepository.Update(toBeUpdatedOrder);
            return _mapper.Map<OrderDTO?>(updatedOrder);
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
