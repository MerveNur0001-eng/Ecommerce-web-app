using Ecommerce.Core.Entities;
using Ecommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class CouponsController : Controller
    {
        private readonly DatabaseContext _context;

        public CouponsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Coupons
        public async Task<IActionResult> Index()
        {
            var coupons = await _context.Coupons.ToListAsync();
            return View(coupons);
        }

        // GET: Admin/Coupons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Coupons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coupon coupon)
        {
            if (!ModelState.IsValid)
                return View(coupon);

            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Coupons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var coupon = await _context.Coupons.FindAsync(id);

            if (coupon == null) return NotFound();

            return View(coupon);
        }

        // POST: Admin/Coupons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Coupon coupon)
        {
            if (id != coupon.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(coupon);

            var dbCoupon = await _context.Coupons.FindAsync(id);
            if (dbCoupon == null) return NotFound();

            dbCoupon.Code = coupon.Code;
            dbCoupon.DiscountPercent = coupon.DiscountPercent;
            dbCoupon.StartDate = coupon.StartDate;
            dbCoupon.EndDate = coupon.EndDate;
            dbCoupon.UsageLimit = coupon.UsageLimit;
            dbCoupon.UsedCount = coupon.UsedCount;
            dbCoupon.IsActive = coupon.IsActive;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Coupons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(x => x.Id == id);

            if (coupon == null) return NotFound();

            return View(coupon);
        }

        // POST: Admin/Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);

            if (coupon != null)
            {
                _context.Coupons.Remove(coupon);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/Coupons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(x => x.Id == id);

            if (coupon == null)
                return NotFound();

            return View(coupon);
        }
    }
}