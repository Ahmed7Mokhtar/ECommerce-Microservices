using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.BusinessLogicLayer.DTOs
{
    public record UpdateProductDTO(Guid ProductId, string ProductName, Category Category, decimal? UnitPrice, int? QuantityInStock)
    {
        public UpdateProductDTO() : this(default, string.Empty, default, default, default)
        {

        }
    }
}
