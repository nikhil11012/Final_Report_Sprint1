using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.Models;

public class Rating
{
    public int Id { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    [Range(1, 5)]
    public int RatingValue { get; set; }

    [MaxLength(500)]
    public string? Comment { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

