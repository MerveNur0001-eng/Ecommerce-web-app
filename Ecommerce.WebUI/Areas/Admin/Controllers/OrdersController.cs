using Ecommerce.Core.Entities;
using Ecommerce.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class OrdersController : Controller
    {
        private readonly DatabaseContext _context;

        public OrdersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.AppUser)
                .ToListAsync();

            return View(orders);
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.AppUser)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (!ModelState.IsValid)
                return View(order);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();

            ViewBag.OrderStates = Enum.GetValues(typeof(EnumOrderState))
                .Cast<EnumOrderState>()
                .Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString()
                });

            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(errors);
            }

            var dbOrder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbOrder == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.OrderStates = Enum.GetValues(typeof(EnumOrderState))
                    .Cast<EnumOrderState>()
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = ((int)x).ToString()
                    });

                return View(order);
            }

            dbOrder.OrderState = order.OrderState;
            dbOrder.BillingAddress = order.BillingAddress;
            dbOrder.DeliveryAddress = order.DeliveryAddress;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order != null)
            {
                if (order.OrderLines != null)
                    _context.OrderLines.RemoveRange(order.OrderLines);

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}