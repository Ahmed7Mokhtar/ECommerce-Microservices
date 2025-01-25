using OrdersService.BusinessLogicLayer.DTOs.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record OrderItemDTO(Guid ProductId, decimal UnitPrice, int Quantity, decimal TotalPrice, string ProductName, string Category)
    {
        public OrderItemDTO() : this(default, default, default, default, "", "")
        {
            
        }
    }
}
