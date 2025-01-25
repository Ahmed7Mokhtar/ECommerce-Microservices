using OrdersService.BusinessLogicLayer.DTOs.Enums;

namespace OrdersService.BusinessLogicLayer.DTOs
{
    public record UserDTO(Guid Id, string Name, string Email, Gender Gender);
}
