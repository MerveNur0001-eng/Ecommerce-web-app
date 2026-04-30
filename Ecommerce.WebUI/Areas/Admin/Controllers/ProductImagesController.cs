using Ecommerce.Core.Entities;
using Ecommerce.Data;
using Ecommerce.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class ProductImagesController : Controller
    {
        private readonly DatabaseContext _context;

        public ProductImagesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductImages
        public async Task<IActionResult> Index(int? productId)
        {
            var databaseContext = _context.ProductImages.Include(p => p.Product);
            if (productId.HasValue)
            {
                return View(await databaseContext.Where(x => x.ProductId ==
                    productId).ToListAsync());
            }
            return View(await databaseContext.ToListAsync());
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var productImage = await _context.ProductImages
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (productImage == null) return NotFound();

            return View(productImage);
        }

        // CREATE GET
        public IActionResult Create(int? productId)
        {
            ViewData["ProductId"] =
                new SelectList(_context.Products, "Id", "Name", productId);

            return View();
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductImage productImage, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] =
                    new SelectList(_context.Products, "Id", "Name", productImage.ProductId);

                return View(productImage);
            }

            if (ImageFile != null)
            {
                productImage.Name =
                    await FileHelper.FileLoaderAsync(ImageFile, "Img/Products");
            }

            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // EDIT GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null) return NotFound();

            ViewData["ProductId"] =
                new SelectList(_context.Products, "Id", "Name", productImage.ProductId);

            return View(productImage);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductImage productImage, IFormFile? ImageFile)
        {
            if (id != productImage.Id) return NotFound();

            var existing = await _context.ProductImages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] =
                    new SelectList(_context.Products, "Id", "Name", productImage.ProductId);

                return View(productImage);
            }

            if (ImageFile != null)
            {
                FileHelper.FileRemover(existing.Name, "Img/Products");

                productImage.Name =
                    await FileHelper.FileLoaderAsync(ImageFile, "Img/Products");
            }
            else
            {
                productImage.Name = existing.Name;
            }

            _context.Update(productImage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var productImage = await _context.ProductImages
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (productImage == null) return NotFound();

            return View(productImage);
        }

        // DELETE POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);

            if (productImage != null)
            {
                FileHelper.FileRemover(productImage.Name, "Img/Products");

                _context.ProductImages.Remove(productImage);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}