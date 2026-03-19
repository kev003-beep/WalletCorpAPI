using WalletCorp.API.Models;
using WalletCorp.API.Models.Helpers;
using WalletCorp.API.Modules.BenefitPlanCategories.DTOs;
using WalletCorp.API.Modules.BenefitPlanCategories.Services.Interfaces;

namespace WalletCorp.API.Modules.BenefitPlanCategories.Services.Implementations;

public class BenefitPlanCategoryService : IBenefitPlanCategoryService
{
    public bool Validate(List<BenefitPlanCategoryDto> categories, out string error)
    {
        error = string.Empty;

        if (categories.Count == 0)
        {
            error = "At least one category is required.";
            return false;
        }

        var used = new HashSet<BenefitCategory>();
        foreach (var item in categories)
        {
            if (item.Category is null)
            {
                error = "Category is required.";
                return false;
            }

            if (!used.Add(item.Category.Value))
            {
                error = "Duplicate categories are not allowed.";
                return false;
            }
        }

        var total = categories.Sum(c => c.Percentage);
        if (Math.Abs(total - 100m) > 0.01m)
        {
            error = "Category percentages must sum to 100.";
            return false;
        }

        return true;
    }

    public List<BenefitPlanCategory> ToEntities(List<BenefitPlanCategoryDto> categories)
    {
        return categories.Select(c => new BenefitPlanCategory
        {
            CategoryName = c.Category?.ToString() ?? string.Empty,
            Percentage = c.Percentage
        }).ToList();
    }

    public List<BenefitPlanCategoryDto> ToDtos(IEnumerable<BenefitPlanCategory> categories)
    {
        return categories.Select(c =>
        {
            var parsed = Enum.TryParse<BenefitCategory>(c.CategoryName, true, out var value)
                ? value
                : BenefitCategory.Grocery;

            return new BenefitPlanCategoryDto
            {
                Category = parsed,
                Percentage = c.Percentage
            };
        }).ToList();
    }
}
