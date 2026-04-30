using Ecommerce.Core.Entities;
using Ecommerce.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebUI.Controllers
{
    [Authorize]
    public class MyAddressesController : Controller
    {
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Address> _serviceAddress;

        public MyAddressesController(IService<AppUser> serviceAppUser, IService<Address> serviceAddress)
        {
            _serviceAppUser = serviceAppUser;
            _serviceAddress = serviceAddress;
        }

        public async Task<IActionResult> Index()
        {
            var userGuidClaim = HttpContext.User.FindFirst("UserGuid")?.Value;

            if (string.IsNullOrEmpty(userGuidClaim))
            {
                return Unauthorized("User session is invalid. Please sign in again.");
            }

            var appUser = await _serviceAppUser.GetAsync(x =>
                x.UserGuid.ToString() == userGuidClaim);

            if (appUser == null)
            {
                return NotFound("User data not found. Please sign out and sign in again.");
            }

            var model = await _serviceAddress.GetAllAsync(u => u.AppUserId == appUser.Id);

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var appUser = await _serviceAppUser.GetAsync(x =>
                        x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);

                    if (appUser != null)
                    {
                        address.AppUserId = appUser.Id;

                        await _serviceAddress.AddAsync(address);
                        await _serviceAddress.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred while saving the address.");
                }
            }

            ModelState.AddModelError("", "Failed to create address. Please check your input.");
            return View(address);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x =>
                x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);

            if (appUser == null)
            {
                return NotFound("User data not found. Please log out and sign in again.");
            }
            var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);

            if (model == null)
            {
                return NotFound("Adres Bilgisi Bulunamadı!");
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Address address)
        {
            var appUser = await _serviceAppUser.GetAsync(x =>
                x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);

            if (appUser == null)
            {
                return NotFound("User data not found. Please log out and sign in again.");
            }
            var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);

            if (model == null)
            {
                return NotFound("Address information not found!");
            }
            model.Title = address.Title;
            model.District = address.District;
            model.City = address.City;
            model.OpenAddress = address.OpenAddress;
            model.IsDeliveryAddress = address.IsDeliveryAddress;
            model.IsBillingAddress = address.IsBillingAddress;
            model.IsActive = address.IsActive;
            var otherAddresses = await _serviceAddress.GetAllAsync(x => x.AppUserId == appUser.Id && x.Id != model.Id);

            foreach (var otherAddress in otherAddresses)
            {
                otherAddress.IsDeliveryAddress = false;
                otherAddress.IsBillingAddress = false;
                _serviceAddress.Update(otherAddress);
            }
            try
            {
                _serviceAddress.Update(model);
                await _serviceAddress.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error Occured!");
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x =>
                x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);

            if (appUser == null)
            {
                return NotFound("User data not found. Please log out and sign in again.");
            }
            var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId == appUser.Id);

            if (model == null)
            {
                return NotFound("Adres Bilgisi Bulunamadı!");
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, Address address)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() ==
                HttpContext.User.FindFirst("UserGuid").Value);

            if (appUser == null)
            {
                return NotFound("User Data Not Found! Please log out and log in again!");
            }

            var model = await _serviceAddress.GetAsync(u => u.AddressGuid.ToString() == id && u.AppUserId ==
                appUser.Id);

            if (model == null)
            {
                return NotFound("Address Information Not Found!");
            }

            try
            {
                _serviceAddress.Delete(model);
                await _serviceAddress.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred!");
            }

            return View(model);
        }
    }
}