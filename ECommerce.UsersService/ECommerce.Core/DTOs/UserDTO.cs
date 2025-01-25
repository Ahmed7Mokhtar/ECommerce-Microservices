using ECommerce.Core.Enums;

namespace ECommerce.Core.DTOs
{
    public record UserDTO(Guid Id, string Name, string Email, Gender Gender);
}
