namespace WalletCorp.API.Models;

public class Wallet
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalBalance { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
    public ICollection<WalletCategory> Categories { get; set; } = new List<WalletCategory>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
