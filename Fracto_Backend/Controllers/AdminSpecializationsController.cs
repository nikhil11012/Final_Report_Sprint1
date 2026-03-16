using Fracto.Api.Data;
using Fracto.Api.DTOs.Specializations;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/admin/specializations")]
[Authorize(Roles = "Admin")]
public class AdminSpecializationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AdminSpecializationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<SpecializationDto>>> GetAll()
    {
        return await _db.Specializations
            .AsNoTracking()
            .Select(s => new SpecializationDto
            {
                SpecializationId = s.Id,
                Name = s.Name,
                Description = s.Description
            })
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<SpecializationDto>> Create([FromBody] SpecializationCreateDto dto)
    {
        var spec = new Specialization
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _db.Specializations.Add(spec);
        await _db.SaveChangesAsync();

        return Ok(new SpecializationDto
        {
            SpecializationId = spec.Id,
            Name = spec.Name,
            Description = spec.Description
        });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SpecializationCreateDto dto)
    {
        var spec = await _db.Specializations.FindAsync(id);
        if (spec == null) return NotFound();

        spec.Name = dto.Name;
        spec.Description = dto.Description;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var spec = await _db.Specializations.FindAsync(id);
        if (spec == null) return NotFound();

        // Check if any doctors are using this specialization
        var hasDoctors = await _db.Doctors.AnyAsync(d => d.SpecializationId == id);
        if (hasDoctors)
        {
            return BadRequest(new { message = "Cannot delete specialization that is assigned to doctors." });
        }

        _db.Specializations.Remove(spec);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

public class SpecializationCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
