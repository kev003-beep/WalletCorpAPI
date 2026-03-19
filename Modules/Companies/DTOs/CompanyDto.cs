namespace WalletCorp.API.Modules.Companies.DTOs;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Rfc { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal GlobalBalanceLimit { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
