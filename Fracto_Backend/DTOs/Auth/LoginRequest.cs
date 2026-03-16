using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.Dtos.Auth;

public class LoginRequest
{
    [Required]
    public string Identifier { get; set; } = string.Empty; // username OR email

    [Required]
    public string Password { get; set; } = string.Empty;
}