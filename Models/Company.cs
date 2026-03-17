namespace WalletCorp.API.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Rfc { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal GlobalBalanceLimit { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<BenefitPlan> BenefitPlans { get; set; } = new List<BenefitPlan>();
}
