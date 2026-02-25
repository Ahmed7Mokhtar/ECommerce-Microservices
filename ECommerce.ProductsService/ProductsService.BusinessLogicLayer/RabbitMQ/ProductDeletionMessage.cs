namespace ProductsService.BusinessLogicLayer.RabbitMQ
{
    public record ProductDeletionMessage(Guid Id, string? Name);
}
