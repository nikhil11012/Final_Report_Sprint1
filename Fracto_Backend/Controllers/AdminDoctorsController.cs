using Fracto.Api.Data;
using Fracto.Api.DTOs.Doctors;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/admin/doctors")]
[Authorize(Roles = "Admin")]
public class AdminDoctorsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public AdminDoctorsController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // POST /api/admin/doctors/upload-image
    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded." });

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "doctors");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        var imagePath = $"/uploads/doctors/{fileName}";
        return Ok(new { imagePath });
    }

    // GET /api/admin/doctors
    [HttpGet]
    public async Task<ActionResult<List<DoctorListItemDto>>> GetAll()
    {
        var items = await _db.Doctors
            .AsNoTracking()
            .Include(d => d.Specialization)
            .OrderBy(d => d.FullName)
            .Select(d => new DoctorListItemDto
            {
                DoctorId = d.Id,
                FullName = d.FullName,
                City = d.City,
                SpecializationName = d.Specialization != null ? d.Specialization.Name : string.Empty,
                AverageRating = d.AverageRating,
                ProfileImagePath = d.ProfileImagePath,
                IsActive = d.IsActive
            })
            .ToListAsync();

        return Ok(items);
    }

    // GET /api/admin/doctors/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoctorListItemDto>> GetById(int id)
    {
        var d = await _db.Doctors
            .Include(d => d.Specialization)
            .SingleOrDefaultAsync(doc => doc.Id == id);

        if (d is null) return NotFound();

        return Ok(new DoctorListItemDto
        {
            DoctorId = d.Id,
            FullName = d.FullName,
            City = d.City,
            SpecializationName = d.Specialization != null ? d.Specialization.Name : string.Empty,
            AverageRating = d.AverageRating,
            ProfileImagePath = d.ProfileImagePath,
            IsActive = d.IsActive
        });
    }

    // POST /api/admin/doctors
    [HttpPost]
    public async Task<ActionResult<DoctorListItemDto>> Create([FromBody] DoctorUpsertDto dto)
    {
        var doctor = new Doctor
        {
            FullName = dto.FullName,
            City = dto.City,
            SpecializationId = dto.SpecializationId,
            IsActive = dto.IsActive,
            ProfileImagePath = dto.ProfileImagePath
        };

        _db.Doctors.Add(doctor);
        await _db.SaveChangesAsync();

        // Reload to get Specialization Name
        return await GetById(doctor.Id);
    }

    // PUT /api/admin/doctors/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] DoctorUpsertDto dto)
    {
        var doctor = await _db.Doctors.FindAsync(id);
        if (doctor is null) return NotFound();

        doctor.FullName = dto.FullName;
        doctor.City = dto.City;
        doctor.SpecializationId = dto.SpecializationId;
        doctor.IsActive = dto.IsActive;
        doctor.ProfileImagePath = dto.ProfileImagePath;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // PUT /api/admin/doctors/{id}/toggle-active
    [HttpPut("{id:int}/toggle-active")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var doctor = await _db.Doctors.FindAsync(id);
        if (doctor is null) return NotFound();

        doctor.IsActive = !doctor.IsActive;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE /api/admin/doctors/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var doctor = await _db.Doctors.FindAsync(id);
        if (doctor is null) return NotFound();

        // Check if doctor has appointments
        var hasAppointments = await _db.Appointments.AnyAsync(a => a.DoctorId == id);
        if (hasAppointments)
        {
            return BadRequest(new { message = "Cannot delete doctor with existing appointments. Deactivate instead." });
        }

        _db.Doctors.Remove(doctor);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}

public class DoctorUpsertDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public int SpecializationId { get; set; }

    [Required]
    public string City { get; set; } = string.Empty;

    public string? ProfileImagePath { get; set; }
    public bool IsActive { get; set; } = true;
}
