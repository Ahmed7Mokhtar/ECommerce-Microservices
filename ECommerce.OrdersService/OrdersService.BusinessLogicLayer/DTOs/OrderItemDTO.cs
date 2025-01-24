using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record OrderItemDTO(Guid ProductId, decimal UnitPrice, int Quantity, decimal TotalPrice)
    {
        public OrderItemDTO() : this(default, default, default, default)
        {
            
        }
    }
}
