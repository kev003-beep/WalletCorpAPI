using System.ComponentModel.DataAnnotations;
using WalletCorp.API.Models.Helpers;

namespace WalletCorp.API.Modules.BenefitPlanCategories.DTOs;

public class BenefitPlanCategoryDto
{
    [Required]
    [EnumDataType(typeof(BenefitCategory))]
    public BenefitCategory? Category { get; set; }

    [Range(0, 100)]
    public decimal Percentage { get; set; }
}
