namespace OrdersService.BusinessLogicLayer.RabbitMQ
{
    public record ProductDeletionMessage(Guid Id, string? Name);
}
