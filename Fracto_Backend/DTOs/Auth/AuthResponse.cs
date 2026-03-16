using Fracto.Api.Models;

namespace Fracto.Api.Dtos.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }

    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string? ProfileImagePath { get; set; }
}