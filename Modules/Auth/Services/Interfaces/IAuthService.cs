using WalletCorp.API.Modules.Auth.DTOs;

namespace WalletCorp.API.Modules.Auth.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
}
