using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record UpdateOrderDTO(Guid OrderId,  Guid UserId, DateTime OrderDate, List<AddOrderItemDTO> Items)
    {
        public UpdateOrderDTO() : this(default, default, default, [])
        {

        }
    }
}
