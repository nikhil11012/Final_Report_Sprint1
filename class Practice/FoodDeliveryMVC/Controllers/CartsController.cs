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
    public class CartsController : Controller
    {
        private readonly FoodDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CartsController(FoodDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var foodDbContext = _context.Carts.Include(c => c.Food).Where(c => c.CustomerId == userId);
            return View(await foodDbContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int foodId, int qty = 1)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var food = await _context.Foods.FindAsync(foodId);
            if (food == null) return NotFound();

            var cartItem = await _context.Carts.FirstOrDefaultAsync(c => c.FoodId == foodId && c.CustomerId == userId);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    FoodId = foodId,
                    Qty = qty,
                    Price = food.Price,
                    TotalAmount = food.Price * qty,
                    CustomerId = userId
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Qty += qty;
                cartItem.TotalAmount = cartItem.Qty * cartItem.Price;
                _context.Update(cartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var cartItems = await _context.Carts.Include(c => c.Food).Where(c => c.CustomerId == userId).ToListAsync();
            if (!cartItems.Any()) return RedirectToAction(nameof(Index));

            var totalAmount = cartItems.Sum(c => c.TotalAmount);

            var sale = new Sale
            {
                CustomerId = userId,
                Date = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending"
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync(); // Save to get SaleId

            foreach (var item in cartItems)
            {
                var productSold = new ProductsSold
                {
                    ProductID = item.FoodId,
                    SaleID = sale.SaleId,
                    Qty = item.Qty,
                    TotalAmount = item.TotalAmount,
                    status = "Sold"
                };
                _context.ProductsSold.Add(productSold);
            }

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Sales");
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Food)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Id");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FoodId,Qty,Price,TotalAmount,CustomerId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Id", cart.FoodId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Id", cart.FoodId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FoodId,Qty,Price,TotalAmount,CustomerId")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            ViewData["FoodId"] = new SelectList(_context.Foods, "Id", "Id", cart.FoodId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Food)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
