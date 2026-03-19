using System.ComponentModel.DataAnnotations;
using WalletCorp.API.Modules.BenefitPlanCategories.DTOs;

namespace WalletCorp.API.Modules.BenefitPlans.DTOs;

public class CreateBenefitPlanDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int CompanyId { get; set; }

    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [Required]
    [MinLength(1)]
    public List<BenefitPlanCategoryDto> Categories { get; set; } = new();
}
