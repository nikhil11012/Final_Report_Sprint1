using System.ComponentModel.DataAnnotations;
using Fracto.Api.Models;

namespace Fracto.Api.Dtos.Auth;

public class RegisterRequest
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MaxLength(150), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}