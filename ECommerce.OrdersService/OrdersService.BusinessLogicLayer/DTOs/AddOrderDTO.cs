namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record AddOrderDTO(Guid UserId, DateTime OrderDate, List<AddOrderItemDTO> Items)
    {
        public AddOrderDTO() : this(default, default, [])
        {
         
        }
    }
}
