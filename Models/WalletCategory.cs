namespace WalletCorp.API.Models;

public class WalletCategory
{
    public int Id { get; set; }
    public int WalletId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal MonthlyLimit { get; set; }

    public Wallet? Wallet { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
