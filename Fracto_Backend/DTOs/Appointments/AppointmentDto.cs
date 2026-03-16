using Fracto.Api.Models;

namespace Fracto.Api.DTOs.Appointments;

public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public AppointmentStatus Status { get; set; }

    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public string DoctorCity { get; set; } = string.Empty;
    public string SpecializationName { get; set; } = string.Empty;
}

