using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.Models;

public enum AppointmentStatus
{
    Booked = 1,
    Approved = 2,
    Completed = 3,
    Cancelled = 4
}

public class Appointment
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public int DoctorId { get; set; }
    public Doctor? Doctor { get; set; }

    [Required]
    public DateOnly AppointmentDate { get; set; }

    [Required, MaxLength(20)]
    public string TimeSlot { get; set; } = string.Empty; // e.g. "10:00-10:30"

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CancelledAtUtc { get; set; }
}

