using Fracto.Api.Data;
using Fracto.Api.DTOs.Specializations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecializationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public SpecializationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<SpecializationDto>>> GetAll()
    {
        var items = await _db.Specializations
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new SpecializationDto
            {
                SpecializationId = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .ToListAsync();

        return Ok(items);
    }
}

