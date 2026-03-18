using System.ComponentModel.DataAnnotations;

namespace WalletCorp.API.Modules.Auth.DTOs;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}
