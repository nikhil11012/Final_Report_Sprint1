using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.Models;

public class Doctor
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    public int SpecializationId { get; set; }
    public Specialization? Specialization { get; set; }

    [Required, MaxLength(100)]
    public string City { get; set; } = string.Empty;

    public decimal? AverageRating { get; set; }

    [MaxLength(300)]
    public string? ProfileImagePath { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}

