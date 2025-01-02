using ECommerce.Core.Enums;

namespace ECommerce.Core.DTOs;

public record AuthResponseDTO(Guid Id, string Email, string Name, Gender Gender, string Token, bool Success)
{
    public AuthResponseDTO() : this(Guid.Empty, string.Empty, string.Empty, Gender.Male, string.Empty, false)
    {
        
    }
}

