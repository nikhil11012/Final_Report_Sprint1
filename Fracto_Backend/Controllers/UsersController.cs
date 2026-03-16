using Fracto.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public UsersController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // POST /api/users/profile-image
    [HttpPost("profile-image")]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded." });

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!int.TryParse(userIdClaim, out var userId)) return Unauthorized();

        var user = await _db.Users.FindAsync(userId);
        if (user is null) return Unauthorized();

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "profiles");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{user.Id}_{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        user.ProfileImagePath = $"/uploads/profiles/{fileName}";
        await _db.SaveChangesAsync();

        return Ok(new { profileImagePath = user.ProfileImagePath });
    }
}
