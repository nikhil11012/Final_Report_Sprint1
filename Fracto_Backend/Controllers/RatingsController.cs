using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fracto.Api.Data;
using Fracto.Api.DTOs.Ratings;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fracto.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RatingsController : ControllerBase
{
    private readonly AppDbContext _db;

    public RatingsController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/ratings?doctorId=1  – public, list of ratings for a doctor
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<RatingDto>>> GetByDoctor([FromQuery] int doctorId)
    {
        var items = await _db.Ratings
            .AsNoTracking()
            .Where(r => r.DoctorId == doctorId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new RatingDto
            {
                RatingId     = r.Id,
                DoctorId     = r.DoctorId,
                UserId       = r.UserId,
                RatingValue  = r.RatingValue,
                Comment      = r.Comment,
                CreatedAtUtc = r.CreatedAtUtc
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<RatingDto>> CreateRating(CreateRatingRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        var doctor = await _db.Doctors.SingleOrDefaultAsync(d => d.Id == request.DoctorId && d.IsActive);
        if (doctor is null)
            return BadRequest(new { message = "Doctor not found or inactive." });

        // Optional rule: user must have at least one non-cancelled appointment with this doctor
        var hasAppointment = await _db.Appointments.AnyAsync(a =>
            a.UserId == userId &&
            a.DoctorId == request.DoctorId &&
            a.Status != AppointmentStatus.Cancelled);

        if (!hasAppointment)
            return BadRequest(new { message = "You can rate a doctor only after having an appointment." });

        // Check if user already rated this doctor (unique index also enforces this)
        var existing = await _db.Ratings.SingleOrDefaultAsync(r =>
            r.DoctorId == request.DoctorId && r.UserId == userId);

        if (existing is null)
        {
            var rating = new Rating
            {
                DoctorId = request.DoctorId,
                UserId = userId.Value,
                RatingValue = request.RatingValue,
                Comment = request.Comment
            };

            _db.Ratings.Add(rating);
            await _db.SaveChangesAsync();
        }
        else
        {
            // Update existing rating
            existing.RatingValue = request.RatingValue;
            existing.Comment = request.Comment;
            await _db.SaveChangesAsync();
        }

        // Recalculate doctor's average rating
        var ratings = await _db.Ratings
            .Where(r => r.DoctorId == request.DoctorId)
            .Select(r => r.RatingValue)
            .ToListAsync();

        doctor.AverageRating = ratings.Count > 0 ? (decimal)ratings.Average() : null;
        await _db.SaveChangesAsync();

        var latestRating = await _db.Ratings
            .AsNoTracking()
            .OrderByDescending(r => r.CreatedAtUtc)
            .FirstOrDefaultAsync(r => r.DoctorId == request.DoctorId && r.UserId == userId);

        if (latestRating is null)
            return StatusCode(500, new { message = "Rating saved but could not be loaded." });

        var dto = new RatingDto
        {
            RatingId = latestRating.Id,
            DoctorId = latestRating.DoctorId,
            UserId = latestRating.UserId,
            RatingValue = latestRating.RatingValue,
            Comment = latestRating.Comment,
            CreatedAtUtc = latestRating.CreatedAtUtc
        };

        return Ok(dto);
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.TryParse(userIdClaim, out var id) ? id : null;
    }
}

