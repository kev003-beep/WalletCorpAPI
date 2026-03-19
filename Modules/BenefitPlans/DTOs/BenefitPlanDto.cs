using WalletCorp.API.Modules.BenefitPlanCategories.DTOs;

namespace WalletCorp.API.Modules.BenefitPlans.DTOs;

public class BenefitPlanDto
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<BenefitPlanCategoryDto> Categories { get; set; } = new();
}
