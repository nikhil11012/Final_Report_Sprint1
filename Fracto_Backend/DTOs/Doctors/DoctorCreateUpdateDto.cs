using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.DTOs.Doctors;

public class DoctorCreateUpdateDto
{
    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public int SpecializationId { get; set; }

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string? ProfileImagePath { get; set; }
}

