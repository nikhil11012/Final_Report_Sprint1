using Fracto.Api.Models;

namespace Fracto.Api.DTOs.Ratings;

public class RatingDto
{
    public int RatingId { get; set; }
    public int DoctorId { get; set; }
    public int UserId { get; set; }
    public int RatingValue { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

