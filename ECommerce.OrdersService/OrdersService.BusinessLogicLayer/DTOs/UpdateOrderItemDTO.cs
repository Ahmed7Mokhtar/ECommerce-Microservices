using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record UpdateOrderItemDTO(Guid ProductId, decimal UnitPrice, int Quantity)
    {
        public UpdateOrderItemDTO() : this(default, default, default)
        {

        }
    }
}
