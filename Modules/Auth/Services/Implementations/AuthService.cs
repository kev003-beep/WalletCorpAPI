using WalletCorp.API.Data;
using WalletCorp.API.Models;
using WalletCorp.API.Helpers;
using WalletCorp.API.Modules.Auth.DTOs;
using WalletCorp.API.Modules.Auth.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WalletCorp.API.Modules.Auth.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly JwtHelper _jwtHelper;

    public AuthService(AppDbContext db, JwtHelper jwtHelper)
    {
        _db = db;
        _jwtHelper = jwtHelper;
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto)
    {
        if (dto.Role is null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Role is required."
            };
        }

        var email = dto.Email.Trim();
        var fullName = dto.FullName.Trim();

        var emailExists = await _db.Users.AnyAsync(u => u.Email == email);
        if (emailExists)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Email already exists."
            };
        }

        var companyExists = await _db.Companies.AnyAsync(c => c.Id == dto.CompanyId);
        if (!companyExists)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Company not found."
            };
        }

        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role.Value.ToString(),
            CompanyId = dto.CompanyId
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _jwtHelper.GenerateToken(user);

        return new LoginResponseDto
        {
            Success = true,
            Message = "Registered successfully.",
            Token = token,
            UserId = user.Id,
            Role = user.Role
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var email = dto.Email.Trim();

        var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid credentials."
            };
        }

        var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!validPassword)
        {
            return new LoginResponseDto
            {
                Success = false,
                Message = "Invalid credentials."
            };
        }

        var token = _jwtHelper.GenerateToken(user);

        return new LoginResponseDto
        {
            Success = true,
            Message = "Login successful.",
            Token = token,
            UserId = user.Id,
            Role = user.Role
        };
    }
}
