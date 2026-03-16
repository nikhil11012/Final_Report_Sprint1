using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.DTOs.Appointments;

public class BookAppointmentRequest
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    public DateOnly AppointmentDate { get; set; }

    [Required, MaxLength(20)]
    public string TimeSlot { get; set; } = string.Empty;
}

