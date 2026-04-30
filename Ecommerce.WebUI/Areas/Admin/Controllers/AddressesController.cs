using Ecommerce.Core.Entities;
using Ecommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class AddressesController : Controller
    {
        private readonly DatabaseContext _context;

        public AddressesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Addresses
        public async Task<IActionResult> Index()
        {
            var addresses = await _context.Addresses
                .Include(a => a.AppUser)
                .ToListAsync();

            return View(addresses);
        }
        public IActionResult Create()
        {
            ViewBag.AppUserId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.AppUsers, "Id", "UserName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AppUserId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.AppUsers, "Id", "Email", address.AppUserId);

            return View(address);
        }

        // GET: Admin/Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var address = await _context.Addresses
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (address == null) return NotFound();

            return View(address);
        }

        // GET: Admin/Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var address = await _context.Addresses
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (address == null) return NotFound();

            return View(address);
        }

        // POST: Admin/Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Addresses.FindAsync(id);

            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}