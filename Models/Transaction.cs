namespace WalletCorp.API.Models;

public class Transaction
{
    public int Id { get; set; }
    public int WalletId { get; set; }
    public int WalletCategoryId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public Wallet? Wallet { get; set; }
    public WalletCategory? WalletCategory { get; set; }
}
