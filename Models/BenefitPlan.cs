namespace WalletCorp.API.Models;

public class BenefitPlan
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    public Company? Company { get; set; }
    public ICollection<BenefitPlanCategory> Categories { get; set; } = new List<BenefitPlanCategory>();
    public ICollection<User> Users { get; set; } = new List<User>();
}
