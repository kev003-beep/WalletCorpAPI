using WalletCorp.API.Modules.BenefitPlans.DTOs;

namespace WalletCorp.API.Modules.BenefitPlans.Services.Interfaces;

public interface IBenefitPlanService
{
    Task<IEnumerable<BenefitPlanDto>> GetAllAsync(int companyId, bool? isActive);
    Task<BenefitPlanDto?> GetByIdAsync(int id, int companyId);
    Task<BenefitPlanDto?> CreateAsync(CreateBenefitPlanDto dto, int companyId);
    Task<bool> AssignAsync(AssignBenefitPlanDto dto, int companyId);
}
