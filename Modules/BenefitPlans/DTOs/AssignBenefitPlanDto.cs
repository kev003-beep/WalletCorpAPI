using System.ComponentModel.DataAnnotations;

namespace WalletCorp.API.Modules.BenefitPlans.DTOs;

public class AssignBenefitPlanDto
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Range(1, int.MaxValue)]
    public int BenefitPlanId { get; set; }
}
