namespace WalletCorp.API.Modules.Auth.DTOs;

public class LoginResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
    public int? UserId { get; set; }
    public string? Role { get; set; }
}
