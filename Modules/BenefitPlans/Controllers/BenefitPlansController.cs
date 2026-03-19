using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletCorp.API.Modules.BenefitPlans.DTOs;
using WalletCorp.API.Modules.BenefitPlans.Services.Interfaces;

namespace WalletCorp.API.Modules.BenefitPlans.Controllers;

[ApiController]
[Route("api/benefitplans")]
[Authorize(Roles = "HRAdmin")]
public class BenefitPlansController : ControllerBase
{
    private readonly IBenefitPlanService _benefitPlanService;

    public BenefitPlansController(IBenefitPlanService benefitPlanService)
    {
        _benefitPlanService = benefitPlanService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
    {
        var companyId = GetCompanyId();
        if (companyId is null)
        {
            return Forbid();
        }

        var result = await _benefitPlanService.GetAllAsync(companyId.Value, isActive);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var companyId = GetCompanyId();
        if (companyId is null)
        {
            return Forbid();
        }

        var result = await _benefitPlanService.GetByIdAsync(id, companyId.Value);
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBenefitPlanDto dto)
    {
        var companyId = GetCompanyId();
        if (companyId is null)
        {
            return Forbid();
        }

        var result = await _benefitPlanService.CreateAsync(dto, companyId.Value);
        if (result is null)
        {
            return BadRequest("Unable to create benefit plan.");
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> Assign([FromBody] AssignBenefitPlanDto dto)
    {
        var companyId = GetCompanyId();
        if (companyId is null)
        {
            return Forbid();
        }

        var assigned = await _benefitPlanService.AssignAsync(dto, companyId.Value);
        if (!assigned)
        {
            return BadRequest("Unable to assign benefit plan.");
        }

        return Ok(new { message = "Benefit plan assigned." });
    }

    private int? GetCompanyId()
    {
        var value = User.FindFirstValue("companyId");
        return int.TryParse(value, out var companyId) ? companyId : null;
    }
}
