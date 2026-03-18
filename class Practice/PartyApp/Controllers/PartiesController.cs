using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyApp.Data;
using PartyApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PartyApp.Controllers
{
    public class PartiesController : Controller
    {
        private readonly PartyDbContext _context;

        public PartiesController(PartyDbContext context)
        {
            _context = context;
        }

        // GET: Parties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parties.ToListAsync());
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var party = await _context.Parties.FirstOrDefaultAsync(m => m.Id == id);
            if (party == null) return NotFound();
            return View(party);
        }

        // GET: Parties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IsExternal")] Party party)
        {
            if (ModelState.IsValid)
            {
                _context.Add(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Parties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var party = await _context.Parties.FindAsync(id);
            if (party == null) return NotFound();
            return View(party);
        }

        // POST: Parties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IsExternal")] Party party)
        {
            if (id != party.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Parties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var party = await _context.Parties.FirstOrDefaultAsync(m => m.Id == id);
            if (party == null) return NotFound();
            return View(party);
        }

        // POST: Parties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party != null) _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 6. Create a method called Signup which returns an empty view.
        // Pass partydropdownlist into the view of this method.
        public async Task<IActionResult> Signup()
        {
            var parties = await _context.Parties.ToListAsync();
            ViewBag.partydropdownlist = new SelectList(parties, "Id", "Name");
            return View();
        }

        // 7. Create a method "AllowSignUp". This method should return a Json, taking in the partyid as parameter.
        [HttpGet]
        public async Task<JsonResult> AllowSignUp(int partyId)
        {
            var party = await _context.Parties.FindAsync(partyId);
            if (party == null) return Json(new { status = "unknown" });
            return Json(new { status = party.IsExternal ? "external" : "internal" });
        }
    }
}
