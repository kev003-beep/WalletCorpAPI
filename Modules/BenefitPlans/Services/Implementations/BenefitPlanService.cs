using Microsoft.EntityFrameworkCore;
using WalletCorp.API.Data;
using WalletCorp.API.Models;
using WalletCorp.API.Modules.BenefitPlanCategories.Services.Interfaces;
using WalletCorp.API.Modules.BenefitPlans.DTOs;
using WalletCorp.API.Modules.BenefitPlans.Services.Interfaces;

namespace WalletCorp.API.Modules.BenefitPlans.Services.Implementations;

public class BenefitPlanService : IBenefitPlanService
{
    private readonly AppDbContext _db;
    private readonly IBenefitPlanCategoryService _categoryService;

    public BenefitPlanService(AppDbContext db, IBenefitPlanCategoryService categoryService)
    {
        _db = db;
        _categoryService = categoryService;
    }

    public async Task<IEnumerable<BenefitPlanDto>> GetAllAsync(int companyId, bool? isActive)
    {
        var query = _db.BenefitPlans
            .AsNoTracking()
            .Include(p => p.Categories)
            .Where(p => p.CompanyId == companyId);

        if (isActive.HasValue)
        {
            query = query.Where(p => p.IsActive == isActive.Value);
        }

        var plans = await query.OrderBy(p => p.Id).ToListAsync();
        return plans.Select(ToDto);
    }

    public async Task<BenefitPlanDto?> GetByIdAsync(int id, int companyId)
    {
        var plan = await _db.BenefitPlans
            .AsNoTracking()
            .Include(p => p.Categories)
            .SingleOrDefaultAsync(p => p.Id == id && p.CompanyId == companyId);

        return plan is null ? null : ToDto(plan);
    }

    public async Task<BenefitPlanDto?> CreateAsync(CreateBenefitPlanDto dto, int companyId)
    {
        if (dto.CompanyId != companyId)
        {
            return null;
        }

        if (!_categoryService.Validate(dto.Categories, out _))
        {
            return null;
        }

        var companyExists = await _db.Companies.AnyAsync(c => c.Id == companyId);
        if (!companyExists)
        {
            return null;
        }

        var plan = new BenefitPlan
        {
            CompanyId = companyId,
            Name = dto.Name.Trim(),
            IsActive = dto.IsActive,
            Categories = _categoryService.ToEntities(dto.Categories)
        };

        _db.BenefitPlans.Add(plan);
        await _db.SaveChangesAsync();

        return ToDto(plan);
    }

    public async Task<bool> AssignAsync(AssignBenefitPlanDto dto, int companyId)
    {
        var plan = await _db.BenefitPlans
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Id == dto.BenefitPlanId && p.CompanyId == companyId);

        if (plan is null)
        {
            return false;
        }

        var user = await _db.Users.SingleOrDefaultAsync(u => u.Id == dto.UserId && u.CompanyId == companyId);
        if (user is null)
        {
            return false;
        }

        user.BenefitPlanId = dto.BenefitPlanId;
        await _db.SaveChangesAsync();

        return true;
    }

    private BenefitPlanDto ToDto(BenefitPlan plan)
    {
        return new BenefitPlanDto
        {
            Id = plan.Id,
            CompanyId = plan.CompanyId,
            Name = plan.Name,
            IsActive = plan.IsActive,
            CreatedAt = plan.CreatedAt,
            Categories = _categoryService.ToDtos(plan.Categories)
        };
    }
}
