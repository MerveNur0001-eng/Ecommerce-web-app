using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Core.Entities;
using Ecommerce.Data;
using Ecommerce.WebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebUI.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Policy = "AdminPolicy")]
    public class NewsController : Controller
    {
        private readonly DatabaseContext _context;

        public NewsController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.News.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var news = await _context.News.FirstOrDefaultAsync(m => m.Id == id);
            if (news == null) return NotFound();

            return View(news);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    news.Image = await FileHelper.FileLoaderAsync(Image, "/Img/News");
                }

                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var news = await _context.News.FindAsync(id);
            if (news == null) return NotFound();

            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News model, IFormFile? Image, bool cbDeleteImage = false)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var entity = await _context.News.FindAsync(id);

            if (entity == null)
                return NotFound();

            // BASIC FIELDS
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.IsActive = model.IsActive; 

            // IMAGE DELETE
            if (cbDeleteImage && !string.IsNullOrEmpty(entity.Image))
            {
                FileHelper.FileRemover(entity.Image, "Img/News");
                entity.Image = string.Empty;
            }

            // IMAGE UPLOAD
            if (Image != null)
            {
                if (!string.IsNullOrEmpty(entity.Image))
                {
                    FileHelper.FileRemover(entity.Image, "Img/News");
                }

                entity.Image = await FileHelper.FileLoaderAsync(Image, "/Img/News");
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var news = await _context.News.FirstOrDefaultAsync(m => m.Id == id);
            if (news == null) return NotFound();

            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                // resim varsa sil
                if (!string.IsNullOrEmpty(news.Image))
                {
                    FileHelper.FileRemover(news.Image, "Img/News");
                }

                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}