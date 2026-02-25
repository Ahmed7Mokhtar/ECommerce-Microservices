namespace ProductsService.BusinessLogicLayer.RabbitMQ
{
    public record ProductNameUpdateMessage(Guid Id, string? NewName);
}
