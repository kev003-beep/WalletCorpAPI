using WalletCorp.API.Modules.Companies.DTOs;

namespace WalletCorp.API.Modules.Companies.Services.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllAsync(bool? isActive);
    Task<CompanyDto?> GetByIdAsync(int id);
    Task<CompanyDto> CreateAsync(CreateCompanyDto dto);
    Task<CompanyDto?> UpdateAsync(int id, CreateCompanyDto dto);
    Task<bool> DeleteAsync(int id);
}
