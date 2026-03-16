using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.DTOs.Ratings;

public class CreateRatingRequest
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    [Range(1, 5)]
    public int RatingValue { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }
}

