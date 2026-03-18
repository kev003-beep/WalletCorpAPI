using System.ComponentModel.DataAnnotations;
using WalletCorp.API.Models.Helpers;

namespace WalletCorp.API.Modules.Users.DTOs;

public class CreateUserDto
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(UserRole))]
    public UserRole? Role { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int CompanyId { get; set; }
}
