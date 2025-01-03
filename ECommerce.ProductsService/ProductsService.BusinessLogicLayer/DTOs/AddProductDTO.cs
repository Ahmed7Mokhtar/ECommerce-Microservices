using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.BusinessLogicLayer.DTOs
{
    public record AddProductDTO(string ProductName, Category Category, decimal? UnitPrice, int? QuantityInStock)
    {
        public AddProductDTO() : this(string.Empty, default, default, default)
        {
            
        }
    }
}
