using ECommerce.Core.Enums;

namespace ECommerce.Core.Entities;

public class AppUser
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Gender Gender { get; set; }
}
