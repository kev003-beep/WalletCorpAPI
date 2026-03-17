namespace WalletCorp.API.Models;

public class BenefitPlanCategory
{
    public int Id { get; set; }
    public int BenefitPlanId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Percentage { get; set; }

    public BenefitPlan? BenefitPlan { get; set; }
}
