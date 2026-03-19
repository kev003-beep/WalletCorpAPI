using WalletCorp.API.Modules.BenefitPlanCategories.DTOs;
using WalletCorp.API.Models;

namespace WalletCorp.API.Modules.BenefitPlanCategories.Services.Interfaces;

public interface IBenefitPlanCategoryService
{
    bool Validate(List<BenefitPlanCategoryDto> categories, out string error);
    List<BenefitPlanCategory> ToEntities(List<BenefitPlanCategoryDto> categories);
    List<BenefitPlanCategoryDto> ToDtos(IEnumerable<BenefitPlanCategory> categories);
}
