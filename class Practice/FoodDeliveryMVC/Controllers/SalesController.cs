using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodDeliveryMVC.Data;
using FoodDeliveryMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace FoodDeliveryMVC.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class SalesController : Controller
    {
        private readonly FoodDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SalesController(FoodDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var sales = await _context.Sales.Where(s => s.CustomerId == userId).ToListAsync();
            return View(sales);
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales
                .FirstOrDefaultAsync(m => m.SaleId == id);
                
            if (sale == null)
            {
                return NotFound();
            }

            ViewBag.Products = await _context.ProductsSold
                .Include(p => p.Food)
                .Where(p => p.SaleID == id)
                .ToListAsync();

            return View(sale);
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SaleId,TotalAmount,CustomerId,Date,Status")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sale);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sale);
        }

        // GET: Sales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SaleId,TotalAmount,CustomerId,Date,Status")] Sale sale)
        {
            if (id != sale.SaleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sale);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaleExists(sale.SaleId))
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
            return View(sale);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var sale = await _context.Sales
                .FirstOrDefaultAsync(m => m.SaleId == id);

            if (sale == null) return NotFound();

            // Security check: Only Admin or the owner can delete
            if (!isAdmin && sale.CustomerId != userId)
            {
                return Unauthorized();
            }

            return View(sale);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Admin");

            var sale = await _context.Sales.FindAsync(id);
            
            if (sale != null)
            {
                // Security check: Only Admin or the owner can delete
                if (!isAdmin && sale.CustomerId != userId)
                {
                    return Unauthorized();
                }

                // Delete related ProductsSold items first
                var relatedProducts = _context.ProductsSold.Where(p => p.SaleID == id);
                _context.ProductsSold.RemoveRange(relatedProducts);

                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.SaleId == id);
        }
    }
}
