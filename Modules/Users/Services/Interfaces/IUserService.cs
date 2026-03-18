using WalletCorp.API.Modules.Users.DTOs;

namespace WalletCorp.API.Modules.Users.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(string? currentRole, int? currentCompanyId);
    Task<UserDto?> GetByIdAsync(int id, string? currentRole, int? currentCompanyId);
    Task<UserDto?> CreateAsync(CreateUserDto dto, string? currentRole, int? currentCompanyId);
    Task<UserDto?> UpdateAsync(int id, UserDto dto, string? currentRole, int? currentCompanyId);
    Task<bool> DeleteAsync(int id, string? currentRole, int? currentCompanyId);
}
