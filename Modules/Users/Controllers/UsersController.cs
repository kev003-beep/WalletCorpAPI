using WalletCorp.API.Modules.Users.DTOs;
using WalletCorp.API.Modules.Users.Services.Interfaces;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WalletCorp.API.Modules.Users.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllAsync(GetRole(), GetCompanyId());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetByIdAsync(id, GetRole(), GetCompanyId());
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var result = await _userService.CreateAsync(dto, GetRole(), GetCompanyId());
        if (result is null)
        {
            return BadRequest("Unable to create user.");
        }

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
    {
        var result = await _userService.UpdateAsync(id, dto, GetRole(), GetCompanyId());
        if (result is null)
        {
            return BadRequest("Unable to update user.");
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id, GetRole(), GetCompanyId());
        if (!deleted)
        {
            return BadRequest("Unable to delete user.");
        }

        return NoContent();
    }

    private string? GetRole()
        => User.FindFirstValue(ClaimTypes.Role);

    private int? GetCompanyId()
    {
        var value = User.FindFirstValue("companyId");
        return int.TryParse(value, out var companyId) ? companyId : null;
    }
}
