using Fracto.Api.Data;
using Fracto.Api.Dtos.Auth;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtOptions _jwt;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(AppDbContext db, IOptions<JwtOptions> jwtOptions, IPasswordHasher<User> passwordHasher)
    {
        _db = db;
        _jwt = jwtOptions.Value;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var username = request.Username.Trim();
        var email = request.Email.Trim().ToLowerInvariant();

        var usernameExists = await _db.Users.AnyAsync(x => x.Username == username);
        if (usernameExists) return BadRequest(new { message = "Username already exists." });

        var emailExists = await _db.Users.AnyAsync(x => x.Email == email);
        if (emailExists) return BadRequest(new { message = "Email already exists." });

        var user = new User
        {
            Username = username,
            Email = email,
            FullName = request.FullName.Trim(),
            Role = UserRole.User
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(CreateAuthResponse(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var identifier = request.Identifier.Trim();
        var identifierLower = identifier.ToLowerInvariant();

        var user = await _db.Users.SingleOrDefaultAsync(x =>
            x.Username == identifier || x.Email == identifierLower);

        if (user is null) return Unauthorized(new { message = "Invalid credentials." });

        var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (verify == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Invalid credentials." });

        return Ok(CreateAuthResponse(user));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> Me()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var user = await _db.Users.FindAsync(userId);
        if (user is null) return Unauthorized();

        return Ok(new CurrentUserResponse
        {
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            FullName = user.FullName,
            ProfileImagePath = user.ProfileImagePath
        });
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(_jwt.ExpiryMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: creds
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expires,
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            ProfileImagePath = user.ProfileImagePath
        };
    }
}