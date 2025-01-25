using OrdersService.BusinessLogicLayer.DTOs.Enums;

namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record ProductDTO(Guid ProductId, string ProductName, string Category, decimal? UnitPrice, int? QuantityInStock);
}
