using Fracto.Api.Models;

public class CurrentUserResponse
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? ProfileImagePath { get; set; }  
}