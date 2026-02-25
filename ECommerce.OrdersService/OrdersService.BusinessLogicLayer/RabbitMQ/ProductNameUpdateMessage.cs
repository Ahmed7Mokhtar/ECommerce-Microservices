namespace OrdersService.BusinessLogicLayer.RabbitMQ
{
    public record ProductNameUpdateMessage(Guid Id, string? NewName);
}
