using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record OrderDTO(Guid OrderId, Guid UserId, decimal TotalBill, DateTime OrderDate, List<OrderItemDTO> Items)
    {
        public OrderDTO() : this(default, default, default, default, new List<OrderItemDTO>())
        {
            
        }
    }
}
