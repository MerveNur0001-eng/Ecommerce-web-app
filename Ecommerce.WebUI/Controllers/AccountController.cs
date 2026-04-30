using System.Security.Claims;
using Ecommerce.Core.Entities;
using Ecommerce.Data;
using Ecommerce.Service.Abstract;
using Ecommerce.WebUI.Models;
using Ecommerce.WebUI.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
namespace Ecommerce.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IService<AppUser> _service;
        private readonly IService<Order> _serviceOrder;
        private readonly MailHelper _mailHelper;

        public AccountController(MailHelper mailHelper, IService<AppUser> service, IService<Order> serviceOrder)
        {
            _mailHelper = mailHelper;
            _service = service;
            _serviceOrder = serviceOrder;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            AppUser user = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
           if (user == null)
            {
                return NotFound();
            }
            var model = new UserEditViewModel()
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Password = user.Password,
                Phone = user.Phone,
                Surname = user.Surname
            };
            return View(model);
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> IndexAsync(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AppUser user = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);


                    if (user is not null)
                    {
                        user.Surname = model.Surname;
                        user.Phone = model.Phone;
                        user.Name = model.Name;
                        user.Password = model.Password;
                        user.Email = model.Email;

                        _service.Update(user);
                        var result =_service.SaveChanges();

                        if (result > 0)
                        {
                            TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
    <strong>Your information has been updated!</strong>
    <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
    </div>";

                            return RedirectToAction("Index");
                        }
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Error Occurred!");
                }

            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            AppUser user = await _service.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);

            if (user == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("SignIn");
            }

            var model = _serviceOrder.GetQueryable()
                .Where(s => s.AppUserId == user.Id)
                .Include(o => o.OrderLines)
                .ThenInclude(p => p.Product);

            return View(model);
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignInAsync(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = await _service.GetAsync(x => x.Email ==
                        loginViewModel.Email && x.Password == loginViewModel.Password && x.IsActive);

                    if (account == null)
                    {
                        ModelState.AddModelError("", "Invalid login attempt!");
                    }
                    else
                    {
                        var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, account.Name),
                    new(ClaimTypes.Role, account.IsAdmin ? "Admin" : "Customer"),
                    new(ClaimTypes.Email, account.Email),
                    new("UserId", account.Id.ToString()),
                    new("UserGuid", account.UserGuid.ToString()),
                };

                        var userIdentity = new ClaimsIdentity(claims, "Login");
                        ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.SignInAsync(userPrincipal);

                        return Redirect(string.IsNullOrEmpty(loginViewModel.ReturnUrl) ? "/" : loginViewModel.ReturnUrl);
                    }
                }
                catch (Exception error)
                {
                    ModelState.AddModelError("", "An error occurred during sign in.");
                }
            }
            return View(loginViewModel);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AppUser appUser)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage);

                return Content(string.Join(" | ", errors));
            }

            appUser.IsAdmin = false;
            appUser.IsActive = true;
            appUser.UserGuid = Guid.NewGuid();
            appUser.CreateDate = DateTime.Now;

            await _service.AddAsync(appUser);
            await _service.SaveChangesAsync();

            return RedirectToAction("SignIn", "Account");
        }
        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("SignIn");
        }
        public IActionResult PasswordRenew()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordRenewAsync(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ModelState.AddModelError("", "Email address cannot be empty!");
                return View();
            }

            AppUser user = await _service.GetAsync(x => x.Email == Email);
            if (user is null)
            {
                ModelState.AddModelError("", "The email you entered could not be found!");
                return View();
            }
            var url = $"{Request.Scheme}://{Request.Host}/Account/PasswordChange?user={user.UserGuid}";

            string message = $"Dear {user.Name} {user.Surname} <br> To reset your password, please <a href='{url}'>Click Here</a>";
            var result = await _mailHelper.SendMailAsync(Email, "Reset My Password", message);

            if (result)
            {
                TempData["Message"] = @"<div class=""alert alert-success alert-dismissible fade show"" role=""alert"">
        <strong>Your password reset link has been sent to your email address!</strong>
        <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
    </div>";
            }
            else
            {
                TempData["Message"] = @"<div class=""alert alert-danger alert-dismissible fade show"" role=""alert"">
        <strong>The password reset link could not be sent to your email address!</strong>
        <button type=""button"" class=""btn-close"" data-bs-dismiss=""alert"" aria-label=""Close""></button>
    </div>";
            }

            return View();
        }

        public async Task<IActionResult> PasswordChangeAsync(string user)
        {
            if (user is null)
            {
                return BadRequest("Invalid Request!");
            }

            AppUser appUser = await _service.GetAsync(x => x.UserGuid.ToString() == user);
            if (appUser is null)
            {
                return NotFound("Invalid Value!");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChange(string user, string Password)
        {
            if (string.IsNullOrEmpty(user))
                return BadRequest("Invalid Request!");

            var appUser = await _service.GetAsync(x => x.UserGuid.ToString() == user);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Invalid Value!");
                return View();
            }

            appUser.Password = Password;

            _service.Update(appUser);

            var result = await _service.SaveChangesAsync();

            if (result > 0)
            {
                TempData["Message"] = "Password updated!";
                return RedirectToAction("SignIn");
            }

            ModelState.AddModelError("", "Update Failed!");
            return View();
        }
    }
    
}
