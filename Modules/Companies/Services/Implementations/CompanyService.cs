using Microsoft.EntityFrameworkCore;
using WalletCorp.API.Data;
using WalletCorp.API.Models;
using WalletCorp.API.Modules.Companies.DTOs;
using WalletCorp.API.Modules.Companies.Services.Interfaces;

namespace WalletCorp.API.Modules.Companies.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly AppDbContext _db;

    public CompanyService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllAsync(bool? isActive)
    {
        var query = _db.Companies.AsNoTracking();

        if (isActive.HasValue)
        {
            query = query.Where(c => c.IsActive == isActive.Value);
        }

        var companies = await query.OrderBy(c => c.Id).ToListAsync();
        return companies.Select(ToDto);
    }

    public async Task<CompanyDto?> GetByIdAsync(int id)
    {
        var company = await _db.Companies.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
        return company is null ? null : ToDto(company);
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto)
    {
        var company = new Company
        {
            Name = dto.Name.Trim(),
            Rfc = dto.Rfc.Trim(),
            Email = dto.Email.Trim(),
            GlobalBalanceLimit = dto.GlobalBalanceLimit,
            IsActive = dto.IsActive
        };

        _db.Companies.Add(company);
        await _db.SaveChangesAsync();

        return ToDto(company);
    }

    public async Task<CompanyDto?> UpdateAsync(int id, CreateCompanyDto dto)
    {
        var company = await _db.Companies.SingleOrDefaultAsync(c => c.Id == id);
        if (company is null)
        {
            return null;
        }

        company.Name = dto.Name.Trim();
        company.Rfc = dto.Rfc.Trim();
        company.Email = dto.Email.Trim();
        company.GlobalBalanceLimit = dto.GlobalBalanceLimit;
        company.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();
        return ToDto(company);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var company = await _db.Companies.SingleOrDefaultAsync(c => c.Id == id);
        if (company is null)
        {
            return false;
        }

        _db.Companies.Remove(company);
        await _db.SaveChangesAsync();
        return true;
    }

    private static CompanyDto ToDto(Company company)
    {
        return new CompanyDto
        {
            Id = company.Id,
            Name = company.Name,
            Rfc = company.Rfc,
            Email = company.Email,
            GlobalBalanceLimit = company.GlobalBalanceLimit,
            IsActive = company.IsActive,
            CreatedAt = company.CreatedAt
        };
    }
}
