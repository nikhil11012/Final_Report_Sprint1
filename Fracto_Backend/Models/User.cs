using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.Models;

public enum UserRole
{
    User = 1,
    Admin = 2
}

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(150), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.User;

    [MaxLength(300)]
    public string? ProfileImagePath { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}