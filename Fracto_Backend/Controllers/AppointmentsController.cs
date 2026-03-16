using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fracto.Api.Data;
using Fracto.Api.DTOs.Appointments;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Fracto.Api.Hubs;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHubContext<NotificationHub> _hubContext;

    public AppointmentsController(AppDbContext db, IHubContext<NotificationHub> hubContext)
    {
        _db = db;
        _hubContext = hubContext;
    }

    [HttpGet("my")]
    public async Task<ActionResult<List<AppointmentDto>>> GetMyAppointments()
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        var items = await _db.Appointments
            .AsNoTracking()
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.Specialization)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.TimeSlot)
            .Select(a => new AppointmentDto
            {
                AppointmentId = a.Id,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                Status = a.Status,
                DoctorId = a.DoctorId,
                DoctorName = a.Doctor != null ? a.Doctor.FullName : string.Empty,
                DoctorCity = a.Doctor != null ? a.Doctor.City : string.Empty,
                SpecializationName = a.Doctor != null && a.Doctor.Specialization != null
                    ? a.Doctor.Specialization.Name
                    : string.Empty
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> BookAppointment(BookAppointmentRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        var doctor = await _db.Doctors
            .Include(d => d.Specialization)
            .SingleOrDefaultAsync(d => d.Id == request.DoctorId);

        if (doctor is null || !doctor.IsActive)
            return BadRequest(new { message = "Doctor not found or inactive." });

        var trimmedSlot = request.TimeSlot.Trim();
        if (string.IsNullOrWhiteSpace(trimmedSlot))
            return BadRequest(new { message = "Time slot is required." });

        // Check for double booking of the same doctor/date/timeslot
        var exists = await _db.Appointments.AnyAsync(a =>
            a.DoctorId == request.DoctorId &&
            a.AppointmentDate == request.AppointmentDate &&
            a.TimeSlot == trimmedSlot &&
            a.Status != AppointmentStatus.Cancelled);

        if (exists)
            return Conflict(new { message = "This time slot is already booked for the selected doctor." });

        var appointment = new Appointment
        {
            UserId = userId.Value,
            DoctorId = request.DoctorId,
            AppointmentDate = request.AppointmentDate,
            TimeSlot = trimmedSlot,
            Status = AppointmentStatus.Booked
        };

        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();

        // SignalR: Notify Admins about the new booking
        var patientName = User.Identity?.Name ?? "A patient";
        await _hubContext.Clients.Group("Admins").SendAsync("ReceiveAdminNotification", $"{patientName} has booked an appointment with Dr. {doctor.FullName}. 📅");

        var dto = new AppointmentDto
        {
            AppointmentId = appointment.Id,
            AppointmentDate = appointment.AppointmentDate,
            TimeSlot = appointment.TimeSlot,
            Status = appointment.Status,
            DoctorId = doctor.Id,
            DoctorName = doctor.FullName,
            DoctorCity = doctor.City,
            SpecializationName = doctor.Specialization?.Name ?? string.Empty
        };

        return CreatedAtAction(nameof(GetMyAppointments), new { }, dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        var appointment = await _db.Appointments.SingleOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        if (appointment is null)
            return NotFound();

        if (appointment.Status == AppointmentStatus.Cancelled)
            return NoContent();

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancelledAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.TryParse(userIdClaim, out var id) ? id : null;
    }
}

