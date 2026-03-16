using Fracto.Api.Data;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminUsersController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/admin/users
    [HttpGet]
    public async Task<ActionResult<List<object>>> GetAll()
    {
        var users = await _db.Users
            .AsNoTracking()
            .OrderByDescending(u => u.CreatedAtUtc)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.FullName,
                u.Role,
                u.ProfileImagePath,
                u.CreatedAtUtc
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET /api/admin/users/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        return Ok(new
        {
            user.Id,
            user.Username,
            user.Email,
            user.FullName,
            user.Role,
            user.ProfileImagePath,
            user.CreatedAtUtc
        });
    }

    // PUT /api/admin/users/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Role = dto.Role;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/admin/users/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user is null) return NotFound();

        // Prevent self-deletion if needed, but for now allow Admin full control
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}

public class UserUpdateDto
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
