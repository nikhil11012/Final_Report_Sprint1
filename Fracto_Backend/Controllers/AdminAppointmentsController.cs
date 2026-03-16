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
[Route("api/admin/appointments")]
[Authorize(Roles = "Admin")]
public class AdminAppointmentsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHubContext<NotificationHub> _hubContext;

    public AdminAppointmentsController(AppDbContext db, IHubContext<NotificationHub> hubContext)
    {
        _db = db;
        _hubContext = hubContext;
    }

    // GET /api/admin/appointments?doctorId=&userId=&date=&status=
    [HttpGet]
    public async Task<ActionResult<List<AdminAppointmentDto>>> GetAll(
        [FromQuery] int? doctorId,
        [FromQuery] int? userId,
        [FromQuery] DateOnly? date,
        [FromQuery] AppointmentStatus? status)
    {
        var query = _db.Appointments
            .AsNoTracking()
            .Include(a => a.User)
            .Include(a => a.Doctor)
            .ThenInclude(d => d!.Specialization)
            .AsQueryable();

        if (doctorId is not null)
            query = query.Where(a => a.DoctorId == doctorId);

        if (userId is not null)
            query = query.Where(a => a.UserId == userId);

        if (date is not null)
            query = query.Where(a => a.AppointmentDate == date);

        if (status is not null)
            query = query.Where(a => a.Status == status);

        var items = await query
            .OrderByDescending(a => a.AppointmentDate)
            .ThenBy(a => a.TimeSlot)
            .Select(a => new AdminAppointmentDto
            {
                AppointmentId = a.Id,
                AppointmentDate = a.AppointmentDate,
                TimeSlot = a.TimeSlot,
                Status = a.Status,
                UserId = a.UserId,
                UserName = a.User != null ? a.User.FullName : string.Empty,
                UserEmail = a.User != null ? a.User.Email : string.Empty,
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

    // Approve an appointment
    // PUT /api/admin/appointments/{id}/approve

    [HttpPut("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var appointment = await _db.Appointments.FindAsync(id);
        if (appointment is null) return NotFound();

        if (appointment.Status == AppointmentStatus.Cancelled)
            return BadRequest(new { message = "Cannot approve a cancelled appointment." });

        appointment.Status = AppointmentStatus.Approved;
        await _db.SaveChangesAsync();

        // SignalR: Notify User
        await _hubContext.Clients.User(appointment.UserId.ToString()).SendAsync("ReceiveNotification", "Your appointment has been approved! ✅");

        return NoContent();
    }

    // Mark an appointment as completed
    // PUT /api/admin/appointments/{id}/complete

    [HttpPut("{id:int}/complete")]
    public async Task<IActionResult> Complete(int id)
    {
        var appointment = await _db.Appointments.FindAsync(id);
        if (appointment is null) return NotFound();

        if (appointment.Status == AppointmentStatus.Cancelled)
            return BadRequest(new { message = "Cannot complete a cancelled appointment." });

        appointment.Status = AppointmentStatus.Completed;
        await _db.SaveChangesAsync();

        // SignalR: Notify User
        await _hubContext.Clients.User(appointment.UserId.ToString()).SendAsync("ReceiveNotification", "Your appointment is complete! You can now rate your doctor. ⭐");

        return NoContent();
    }

    // Admin-side cancel (override)
    // PUT /api/admin/appointments/{id}/cancel
    
    [HttpPut("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var appointment = await _db.Appointments.FindAsync(id);
        if (appointment is null) return NotFound();

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancelledAtUtc = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // SignalR: Notify User
        await _hubContext.Clients.User(appointment.UserId.ToString()).SendAsync("ReceiveNotification", "Your appointment has been cancelled by the administrator. ❌");

        return NoContent();
    }
}

