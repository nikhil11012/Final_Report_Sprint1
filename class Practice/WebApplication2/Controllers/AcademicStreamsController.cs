using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AcademicStreamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicStreamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AcademicStreams
        public async Task<IActionResult> Index()
        {
            return View(await _context.AcademicStreams.ToListAsync());
        }

        // GET: AcademicStreams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicStream = await _context.AcademicStreams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicStream == null)
            {
                return NotFound();
            }

            return View(academicStream);
        }

        // GET: AcademicStreams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AcademicStreams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] AcademicStream academicStream)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicStream);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academicStream);
        }

        // GET: AcademicStreams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicStream = await _context.AcademicStreams.FindAsync(id);
            if (academicStream == null)
            {
                return NotFound();
            }
            return View(academicStream);
        }

        // POST: AcademicStreams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] AcademicStream academicStream)
        {
            if (id != academicStream.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicStream);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicStreamExists(academicStream.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(academicStream);
        }

        // GET: AcademicStreams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicStream = await _context.AcademicStreams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (academicStream == null)
            {
                return NotFound();
            }

            return View(academicStream);
        }

        // POST: AcademicStreams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academicStream = await _context.AcademicStreams.FindAsync(id);
            if (academicStream != null)
            {
                _context.AcademicStreams.Remove(academicStream);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicStreamExists(int id)
        {
            return _context.AcademicStreams.Any(e => e.Id == id);
        }
    }
}
