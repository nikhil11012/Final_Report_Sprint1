using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using WebApplication21.Models;

namespace WebApplication21.Controllers.NewFolder
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmaciesController : ControllerBase
    {
        private readonly PharmacyContext _context;

        public PharmaciesController(PharmacyContext context)
        {
            _context = context;
        }

        // GET: api/Pharmacies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pharmacy>>> GetPharmacies()
        {
            return await _context.Pharmacies.ToListAsync();
        }

        // GET: api/Pharmacies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pharmacy>> GetPharmacy(int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);

            if (pharmacy == null)
            {
                return NotFound();
            }

            return pharmacy;
        }

        // PUT: api/Pharmacies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPharmacy(int id, Pharmacy pharmacy)
        {
            if (id != pharmacy.Id)
            {
                return BadRequest();
            }

            _context.Entry(pharmacy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PharmacyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Pharmacies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pharmacy>> PostPharmacy(Pharmacy pharmacy)
        {
            _context.Pharmacies.Add(pharmacy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPharmacy", new { id = pharmacy.Id }, pharmacy);
        }

        // DELETE: api/Pharmacies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePharmacy(int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            _context.Pharmacies.Remove(pharmacy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PharmacyExists(int id)
        {
            return _context.Pharmacies.Any(e => e.Id == id);
        }
    }
}
