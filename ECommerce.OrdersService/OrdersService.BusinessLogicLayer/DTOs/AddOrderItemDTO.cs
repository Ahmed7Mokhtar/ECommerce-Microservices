namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record AddOrderItemDTO(Guid ProductId, decimal UnitPrice, int Quantity)
    {
        public AddOrderItemDTO(): this(default, default, default)
        {
            
        }
    }
}
