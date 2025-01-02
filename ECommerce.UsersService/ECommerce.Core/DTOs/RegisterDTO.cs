using ECommerce.Core.Enums;

namespace ECommerce.Core.DTOs;

public record RegisterDTO(string Email, string Password, string Name, Gender Gender);

