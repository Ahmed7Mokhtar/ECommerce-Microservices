using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsService.BusinessLogicLayer.DTOs
{
    public record ProductResponseDTO(Guid ProductId, string ProductName, Category Category, decimal? UnitPrice, int? QuantityInStock)
    {
        public ProductResponseDTO() : this(default, default, default, default, default)
        {

        }
    }
}
