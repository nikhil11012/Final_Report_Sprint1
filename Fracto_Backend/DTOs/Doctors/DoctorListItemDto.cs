namespace Fracto.Api.DTOs.Doctors;

public class DoctorListItemDto
{
    public int DoctorId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public decimal? AverageRating { get; set; }
    public bool IsActive { get; set; }

    public int SpecializationId { get; set; }
    public string SpecializationName { get; set; } = string.Empty;
    public string? ProfileImagePath { get; set; }
}

