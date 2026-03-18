using Microsoft.EntityFrameworkCore;
using WalletCorp.API.Data;
using WalletCorp.API.Models;
using WalletCorp.API.Models.Helpers;
using WalletCorp.API.Modules.Users.DTOs;
using WalletCorp.API.Modules.Users.Services.Interfaces;

namespace WalletCorp.API.Modules.Users.Services.Implementations;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(string? currentRole, int? currentCompanyId)
    {
        var query = _db.Users.AsNoTracking();

        if (!IsSuperAdmin(currentRole) && currentCompanyId is not null)
        {
            query = query.Where(u => u.CompanyId == currentCompanyId.Value);
        }

        var users = await query.OrderBy(u => u.Id).ToListAsync();
        return users.Select(ToDto);
    }

    public async Task<UserDto?> GetByIdAsync(int id, string? currentRole, int? currentCompanyId)
    {
        var user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return null;
        }

        if (!IsSuperAdmin(currentRole) && currentCompanyId is not null && user.CompanyId != currentCompanyId.Value)
        {
            return null;
        }

        return ToDto(user);
    }

    public async Task<UserDto?> CreateAsync(CreateUserDto dto, string? currentRole, int? currentCompanyId)
    {
        if (dto.Role is null)
        {
            return null;
        }

        if (!IsSuperAdmin(currentRole) && !IsHrAdmin(currentRole))
        {
            return null;
        }

        var targetCompanyId = dto.CompanyId;
        if (!IsSuperAdmin(currentRole) && currentCompanyId is not null)
        {
            targetCompanyId = currentCompanyId.Value;
        }

        var companyExists = await _db.Companies.AnyAsync(c => c.Id == targetCompanyId);
        if (!companyExists)
        {
            return null;
        }

        var email = dto.Email.Trim();
        var emailExists = await _db.Users.AnyAsync(u => u.Email == email);
        if (emailExists)
        {
            return null;
        }

        var user = new User
        {
            FullName = dto.FullName.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role.Value.ToString(),
            CompanyId = targetCompanyId
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return ToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(int id, UserDto dto, string? currentRole, int? currentCompanyId)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return null;
        }

        if (!IsSuperAdmin(currentRole) && currentCompanyId is not null && user.CompanyId != currentCompanyId.Value)
        {
            return null;
        }

        if (!IsSuperAdmin(currentRole) && !IsHrAdmin(currentRole))
        {
            return null;
        }

        user.FullName = dto.FullName.Trim();
        user.Email = dto.Email.Trim();
        user.Role = dto.Role.ToString();

        if (IsSuperAdmin(currentRole))
        {
            user.CompanyId = dto.CompanyId;
        }

        await _db.SaveChangesAsync();
        return ToDto(user);
    }

    public async Task<bool> DeleteAsync(int id, string? currentRole, int? currentCompanyId)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user is null)
        {
            return false;
        }

        if (!IsSuperAdmin(currentRole) && currentCompanyId is not null && user.CompanyId != currentCompanyId.Value)
        {
            return false;
        }

        if (!IsSuperAdmin(currentRole) && !IsHrAdmin(currentRole))
        {
            return false;
        }

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    private static bool IsSuperAdmin(string? role)
        => string.Equals(role, UserRole.SuperAdmin.ToString(), StringComparison.OrdinalIgnoreCase);

    private static bool IsHrAdmin(string? role)
        => string.Equals(role, UserRole.HRAdmin.ToString(), StringComparison.OrdinalIgnoreCase);

    private static UserDto ToDto(User user)
    {
        var role = Enum.TryParse<UserRole>(user.Role, true, out var parsedRole)
            ? parsedRole
            : UserRole.Employee;

        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = role,
            CompanyId = user.CompanyId,
            CreatedAt = user.CreatedAt
        };
    }
}
