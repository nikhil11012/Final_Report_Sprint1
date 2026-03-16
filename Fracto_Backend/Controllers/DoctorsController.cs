using Fracto.Api.Data;
using Fracto.Api.DTOs.Doctors;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly AppDbContext _db;

    public DoctorsController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/doctors?city=delhi&specializationId=1&minRating=4
    [HttpGet]
    public async Task<ActionResult<List<DoctorListItemDto>>> GetAll(
        [FromQuery] string? city,
        [FromQuery] int? specializationId,
        [FromQuery] decimal? minRating,
        [FromQuery] bool onlyActive = true)
    {
        var query = _db.Doctors
            .AsNoTracking()
            .Include(x => x.Specialization)
            .AsQueryable();

        if (onlyActive)
            query = query.Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(city))
        {
            var cityTrim = city.Trim();
            query = query.Where(x => x.City == cityTrim);
        }

        if (specializationId is not null)
            query = query.Where(x => x.SpecializationId == specializationId);

        if (minRating is not null)
            query = query.Where(x => x.AverageRating != null && x.AverageRating >= minRating);

        var items = await query
            .OrderBy(x => x.FullName)
            .Select(x => new DoctorListItemDto
            {
                DoctorId = x.Id,
                FullName = x.FullName,
                City = x.City,
                AverageRating = x.AverageRating,
                IsActive = x.IsActive,
                SpecializationId = x.SpecializationId,
                SpecializationName = x.Specialization != null ? x.Specialization.Name : string.Empty,
                ProfileImagePath = x.ProfileImagePath
            })
            .ToListAsync();

        return Ok(items);
    }

    // GET /api/doctors/cities  – distinct city list for dropdown
    [HttpGet("cities")]
    public async Task<ActionResult<List<string>>> GetCities()
    {
        var cities = await _db.Doctors
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Select(x => x.City)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        return Ok(cities);
    }

    // GET /api/doctors/{id}  – single doctor detail
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoctorListItemDto>> GetById(int id)
    {
        var doctor = await _db.Doctors
            .AsNoTracking()
            .Include(x => x.Specialization)
            .Where(x => x.Id == id)
            .Select(x => new DoctorListItemDto
            {
                DoctorId      = x.Id,
                FullName      = x.FullName,
                City          = x.City,
                AverageRating = x.AverageRating,
                IsActive      = x.IsActive,
                SpecializationId   = x.SpecializationId,
                SpecializationName = x.Specialization != null ? x.Specialization.Name : string.Empty,
                ProfileImagePath = x.ProfileImagePath
            })
            .FirstOrDefaultAsync();

        if (doctor is null) return NotFound();
        return Ok(doctor);
    }

    // GET /api/doctors/{id}/available-slots?date=2025-06-01
    // Returns the list of standard 30-min slots that are NOT already booked
    [HttpGet("{id:int}/available-slots")]
    public async Task<ActionResult<List<string>>> GetAvailableSlots(int id, [FromQuery] DateOnly date)
    {
        var doctorExists = await _db.Doctors.AnyAsync(d => d.Id == id && d.IsActive);
        if (!doctorExists) return NotFound(new { message = "Doctor not found or inactive." });

        // Standard clinic hours: 09:00 – 17:00, 30-min slots
        var allSlots = GenerateSlots(new TimeOnly(9, 0), new TimeOnly(17, 0), 30);

        var bookedSlots = await _db.Appointments
            .AsNoTracking()
            .Where(a => a.DoctorId == id
                     && a.AppointmentDate == date
                     && a.Status != AppointmentStatus.Cancelled)
            .Select(a => a.TimeSlot)
            .ToListAsync();

        var available = allSlots.Except(bookedSlots).ToList();
        return Ok(available);
    }

    private static List<string> GenerateSlots(TimeOnly start, TimeOnly end, int stepMinutes)
    {
        var slots = new List<string>();
        var current = start;
        while (current < end)
        {
            var next = current.AddMinutes(stepMinutes);
            slots.Add($"{current:HH:mm}-{next:HH:mm}");
            current = next;
        }
        return slots;
    }

    // ADMIN: get all doctors including inactive
    // GET /api/doctors/admin
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<DoctorListItemDto>>> GetAllForAdmin()
    {
        var items = await _db.Doctors
            .AsNoTracking()
            .Include(x => x.Specialization)
            .OrderBy(x => x.FullName)
            .Select(x => new DoctorListItemDto
            {
                DoctorId = x.Id,
                FullName = x.FullName,
                City = x.City,
                AverageRating = x.AverageRating,
                IsActive = x.IsActive,
                SpecializationId = x.SpecializationId,
                SpecializationName = x.Specialization != null ? x.Specialization.Name : string.Empty,
                ProfileImagePath = x.ProfileImagePath
            })
            .ToListAsync();

        return Ok(items);
    }

    // ADMIN: create a new doctor
    // POST /api/doctors
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<DoctorListItemDto>> CreateDoctor(DoctorCreateUpdateDto dto)
    {
        var specializationExists = await _db.Specializations.AnyAsync(s => s.Id == dto.SpecializationId);
        if (!specializationExists)
            return BadRequest(new { message = "Specialization not found." });

        var doctor = new Doctor
        {
            FullName = dto.FullName.Trim(),
            City = dto.City.Trim(),
            SpecializationId = dto.SpecializationId,
            IsActive = dto.IsActive,
            ProfileImagePath = dto.ProfileImagePath
        };

        _db.Doctors.Add(doctor);
        await _db.SaveChangesAsync();

        var specialization = await _db.Specializations.FindAsync(dto.SpecializationId);

        var result = new DoctorListItemDto
        {
            DoctorId = doctor.Id,
            FullName = doctor.FullName,
            City = doctor.City,
            AverageRating = doctor.AverageRating,
            IsActive = doctor.IsActive,
            SpecializationId = doctor.SpecializationId,
            SpecializationName = specialization?.Name ?? string.Empty,
            ProfileImagePath = doctor.ProfileImagePath
        };

        return CreatedAtAction(nameof(GetAllForAdmin), new { id = doctor.Id }, result);
    }

    // ADMIN: update an existing doctor
    // PUT /api/doctors/{id}
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDoctor(int id, DoctorCreateUpdateDto dto)
    {
        var doctor = await _db.Doctors.FindAsync(id);
        if (doctor is null) return NotFound();

        var specializationExists = await _db.Specializations.AnyAsync(s => s.Id == dto.SpecializationId);
        if (!specializationExists)
            return BadRequest(new { message = "Specialization not found." });

        doctor.FullName = dto.FullName.Trim();
        doctor.City = dto.City.Trim();
        doctor.SpecializationId = dto.SpecializationId;
        doctor.IsActive = dto.IsActive;
        doctor.ProfileImagePath = dto.ProfileImagePath;

        await _db.SaveChangesAsync();

        return NoContent();
    }

    // ADMIN: soft delete / deactivate doctor
    // PUT /api/doctors/{id}/deactivate
    [HttpPut("{id:int}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateDoctor(int id)
    {
        var doctor = await _db.Doctors.FindAsync(id);
        if (doctor is null) return NotFound();

        doctor.IsActive = false;
        await _db.SaveChangesAsync();

        return NoContent();
    }
}

