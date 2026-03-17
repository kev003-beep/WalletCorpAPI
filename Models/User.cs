namespace WalletCorp.API.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int CompanyId { get; set; }
    public DateTime CreatedAt { get; set; }

    public Company? Company { get; set; }
    public Wallet? Wallet { get; set; }
    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
