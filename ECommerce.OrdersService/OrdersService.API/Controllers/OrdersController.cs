using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using OrdersService.BusinessLogicLayer.DTOs;
using OrdersService.BusinessLogicLayer.ServiceContracts;
using OrdersService.DataAccessLayer.Entities;

namespace OrdersService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> Get()
        {
            var result = await _ordersService.GetAll();
            return Ok(result);
        }

        [HttpGet("search/productid/{productId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllByProductId(Guid productId)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(m => m.OrderItems, Builders<OrderItem>.Filter.Eq(i => i.ProductId, productId));
            var result = await _ordersService.GetOrdersByCondition(filter);
            return Ok(result);
        }

        [HttpGet("search/orderdate/{orderDate}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllByOrderDate(DateTime orderDate)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(m => m.OrderDate.ToString("yyy-MM-dd"), orderDate.ToString("yyy-MM-dd"));
            var result = await _ordersService.GetOrdersByCondition(filter);
            return Ok(result);
        }

        [HttpGet("search/userid/{userid}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllByUserId(Guid userid)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(m => m.UserId, userid);
            var result = await _ordersService.GetOrdersByCondition(filter);
            return Ok(result);
        }

        [HttpGet("search/orderid/{id}")]
        public async Task<ActionResult<OrderDTO>> GetById(Guid id)
        {
            FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(m => m.OrderId, id);
            var result = await _ordersService.GetOrderByCondition(filter);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Add(AddOrderDTO addOrderDTO)
        {
            if (addOrderDTO is null)
                return BadRequest("Invalid order data!");

            var result = await _ordersService.Add(addOrderDTO);
            if (result is null)
                return Problem("Problem in adding order!");

            return CreatedAtAction(nameof(GetById), new { id = result.OrderId }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDTO>> Update(UpdateOrderDTO updateOrderDTO, Guid id)
        {
            if (updateOrderDTO is null)
                return BadRequest("Invalid order data!");

            if(updateOrderDTO.OrderId != id)
                return BadRequest("Invalid order Id!");

            var result = await _ordersService.Update(updateOrderDTO);
            if (result is null)
                return Problem("Problem in updating order!");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OrderDTO>> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid order id!");

            var result = await _ordersService.Delete(id);
            if (!result)
                return Problem("Problem in deleting order!");

            return Ok(result);
        }
    }
}
