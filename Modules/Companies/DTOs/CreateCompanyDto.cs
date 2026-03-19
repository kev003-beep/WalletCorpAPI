using System.ComponentModel.DataAnnotations;

namespace WalletCorp.API.Modules.Companies.DTOs;

public class CreateCompanyDto
{
    [Required]
    [StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 8)]
    public string Rfc { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal GlobalBalanceLimit { get; set; }

    public bool IsActive { get; set; } = true;
}
