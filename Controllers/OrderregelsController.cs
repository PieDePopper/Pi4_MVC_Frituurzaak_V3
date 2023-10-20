using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pi4_MVC_Frituur_V3.Models;
using Pi4_MVC_Frituurzaak_V3.Data;

namespace Pi4_MVC_Frituurzaak_V3.Controllers
{
    public class OrderregelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderregelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orderregels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orderregel.Include(o => o.Item);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Orderregels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orderregel == null)
            {
                return NotFound();
            }

            var orderregel = await _context.Orderregel
                .Include(o => o.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderregel == null)
            {
                return NotFound();
            }

            return View(orderregel);
        }

        // GET: Orderregels/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id");
            return View();
        }

        // POST: Orderregels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,Quantity")] Orderregel orderregel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderregel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", orderregel.ItemId);
            return View(orderregel);
        }


        [HttpPost]
        public IActionResult AddToCart(int itemId)
        {
            // Zoek het Item in de database op basis van itemId
            var item = _context.Item.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                return Json(new { success = false });
            }

            // Hier voeg je de gegevens toe aan de database (OrderRegel) en sla je deze op
            // Je kunt de huidige gebruiker identificeren met UserManager.GetUserId(User)

            // Voorbeeld:
            var orderRegel = new Orderregel
            {
                ItemId = item.Id,
                Quantity = 1,
                Item = item
            };

            // Voeg de orderregel toe aan de huidige bestelling van de gebruiker
            // Sla wijzigingen op in de database

            return Json(new { success = true });
        }



        // GET: Orderregels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orderregel == null)
            {
                return NotFound();
            }

            var orderregel = await _context.Orderregel.FindAsync(id);
            if (orderregel == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", orderregel.ItemId);
            return View(orderregel);
        }

        // POST: Orderregels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,Quantity")] Orderregel orderregel)
        {
            if (id != orderregel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderregel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderregelExists(orderregel.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", orderregel.ItemId);
            return View(orderregel);
        }

        // GET: Orderregels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orderregel == null)
            {
                return NotFound();
            }

            var orderregel = await _context.Orderregel
                .Include(o => o.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderregel == null)
            {
                return NotFound();
            }

            return View(orderregel);
        }

        // POST: Orderregels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orderregel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orderregel'  is null.");
            }
            var orderregel = await _context.Orderregel.FindAsync(id);
            if (orderregel != null)
            {
                _context.Orderregel.Remove(orderregel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderregelExists(int id)
        {
          return (_context.Orderregel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
