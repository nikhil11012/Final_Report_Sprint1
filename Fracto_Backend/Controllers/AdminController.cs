using Fracto.Api.Data;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminController(AppDbContext db)
    {
        _db = db;
    }
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var totalUsers = await _db.Users.CountAsync();
        var totalDoctors = await _db.Doctors.CountAsync();
        var totalAppointments = await _db.Appointments.CountAsync();
        var pendingAppointments = await _db.Appointments.CountAsync(a => a.Status == AppointmentStatus.Booked);

        return Ok(new
        {
            TotalUsers = totalUsers,
            TotalDoctors = totalDoctors,
            TotalAppointments = totalAppointments,
            PendingAppointments = pendingAppointments
        });
    }
}