using Ecommerce.Core.Entities;
using Ecommerce.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebUI.Controllers
{
    public class NewsController : Controller
    {
        private readonly IService<News> _service;

        public NewsController(IService<News> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var filtered = await _service.GetAllAsync(x => x.IsActive);

            return View(
                filtered
                    .OrderByDescending(x => x.Id)
                    .ToList()
            );
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound("Invalid Request!");

            var news = await _service.GetAsync(x => x.Id == id && x.IsActive);

            if (news == null)
                return NotFound("No Valid Campaign Found!");

            return View(news);
        }
    }
}