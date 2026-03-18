using WalletCorp.API.Models.Helpers;

namespace WalletCorp.API.Modules.Users.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }
}
