using System.Globalization;
using Ecommerce.Core.Entities;
using Ecommerce.Data;
using Ecommerce.Service.Abstract;
using Ecommerce.Service.Concrete;
using Ecommerce.WebUI.ExtensionMethods;
using Ecommerce.WebUI.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IService<Product> _serviceProduct;
        private readonly IService<Core.Entities.Address> _serviceAddress;
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Order> _serviceOrder;
        private readonly IConfiguration _configuration;
        public CartController(IService<Product> serviceProduct, IService<Core.Entities.Address> serviceAddress, IService<AppUser> serviceAppUser, IService<Order> serviceOrder, IConfiguration configuration, DatabaseContext context)
        {
            _serviceProduct = serviceProduct;
            _serviceAddress = serviceAddress;
            _serviceAppUser = serviceAppUser;
            _serviceOrder = serviceOrder;
            _configuration = configuration;
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            var model = new CartViewModel
            {
                CartLines = cart.CartLines,
                TotalPrice = cart.TotalPrice()
            };
            return View(model);
        }
        public IActionResult Thanks()
        {
            return View();
        }
        public IActionResult Add(int ProductId, int quantity = 1, string returnUrl = null)
        {
            var product = _serviceProduct.Find(ProductId);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddProduct(product, quantity);
                HttpContext.Session.SetJson("Cart", cart);
            }

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Products");
        }
        public IActionResult Update(int ProductId, int quantity = 1)
        {
            var product = _serviceProduct.Find(ProductId);
            if (product != null)
            {
                var cart = GetCart();
                cart.UpdateProduct(product, quantity);
                HttpContext.Session.SetJson("Cart", cart);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int ProductId)
        {
            var product = _serviceProduct.Find(ProductId);
            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveProduct(product);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = GetCart();

            var userGuid = HttpContext.User.FindFirst("UserGuid")?.Value;

            if (string.IsNullOrEmpty(userGuid))
            {
                return RedirectToAction("SignIn", "Account");
            }

            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == userGuid);

            if (appUser == null)
            {
                return RedirectToAction("SignIn", "Account");
            }

            var addresses = await _serviceAddress
                .GetAllAsync(a => a.AppUserId == appUser.Id && a.IsActive);

            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.TotalPrice(),
                DiscountAmount = cart.DiscountAmount(),
                FinalPrice = cart.FinalPrice(),
                CouponCode = cart.CouponCode,
                Addresses = addresses
            };

            return View(model);
        }
        [Authorize, HttpPost]
        public async Task<IActionResult> Checkout(string CardNameSurname,string CardNumber, string CardMonth, string CardYear, string CVV, string DeliveryAddress, string BillingAddress)
        {
            var cart = GetCart();
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);

            if (appUser == null)
            {
                return RedirectToAction("SignIn", "Account");
            }

            var addresses = await _serviceAddress.GetAllAsync(a => a.AppUserId == appUser.Id && a.IsActive);
            var model = new CheckoutViewModel()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.FinalPrice(),
                Addresses = addresses
            };

            if (string.IsNullOrWhiteSpace(CardNumber) || string.IsNullOrWhiteSpace(CardMonth) ||
                string.IsNullOrWhiteSpace(CardYear) || string.IsNullOrWhiteSpace(CVV) ||
                string.IsNullOrWhiteSpace(DeliveryAddress) || string.IsNullOrWhiteSpace(BillingAddress))
            {
                return View(model);
            }
            var faturaAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == BillingAddress);

            var teslimatAdresi = addresses.FirstOrDefault(a => a.AddressGuid.ToString() == DeliveryAddress);


            var order = new Order
            {
                AppUserId = appUser.Id,
                BillingAddress = $"{faturaAdresi.OpenAddress} {faturaAdresi.District} {faturaAdresi.City}",
                DeliveryAddress = $"{teslimatAdresi.OpenAddress} {teslimatAdresi.District} {teslimatAdresi.City}",
                CustomerId = appUser.UserGuid.ToString(),
                OrderDate = DateTime.Now,
                TotalPrice = cart.FinalPrice(),
                OrderNumber = Guid.NewGuid().ToString(),
                OrderState = 0,
                OrderLines = []
            };

            #region PaymentProcess
            Options options = new Options();
            options.ApiKey = _configuration["IyzicOptions:ApiKey"];
            options.SecretKey = _configuration["IyzicOptions:SecretKey"];
            options.BaseUrl = _configuration["IyzicOptions:BaseUrl"];    //"https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = HttpContext.Session.Id;
            request.Price = cart.FinalPrice().ToString().Replace(",", ".");
            request.PaidPrice = cart.FinalPrice().ToString().Replace(",", ".");
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B" + HttpContext.Session.Id; ;
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = CardNameSurname; //"John Doe";
            paymentCard.CardNumber = CardNumber; //"5528790000000008";
            paymentCard.ExpireMonth = CardMonth; //"12";
            paymentCard.ExpireYear = CardYear; //"2030";
            paymentCard.Cvc = CVV; // "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY" + appUser.Id;
            buyer.Name = appUser.Name;
            buyer.Surname = appUser.Surname;
            buyer.GsmNumber = appUser.Phone;
            buyer.Email = appUser.Email;
            buyer.IdentityNumber = "11111111111";
            buyer.LastLoginDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"); //"2015-10-05 12:43:35";
            buyer.RegistrationDate = appUser.CreateDate.ToString("yyyy-mm-dd hh:mm:ss"); //"2013-04-21 15:12:09";
            buyer.RegistrationAddress = order.DeliveryAddress;
            buyer.Ip = HttpContext.Connection.RemoteIpAddress?.ToString(); //"85.34.78.112";
            buyer.City = teslimatAdresi.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            var shippingAddress = new Iyzipay.Model.Address();
            shippingAddress.ContactName = appUser.Name + " " + appUser.Surname;
            shippingAddress.City = teslimatAdresi.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = teslimatAdresi.OpenAddress;
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            var billingAddress = new Iyzipay.Model.Address();
            billingAddress.ContactName = appUser.Name + " " + appUser.Surname;
            billingAddress.City = faturaAdresi.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = faturaAdresi.OpenAddress;
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            decimal productTotal = 0;
            decimal shipping = 0;
            decimal discount = cart.DiscountAmount();

            List<BasketItem> basketItems = new List<BasketItem>();

            foreach (var item in cart.CartLines)
            {
                var itemTotal = item.Product.Price * item.Quantity;
                productTotal += itemTotal;

                order.OrderLines.Add(new OrderLine
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });

                basketItems.Add(new BasketItem
                {
                    Id = item.Product.Id.ToString(),
                    Name = item.Product.Name,
                    Category1 = "Product",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = itemTotal.ToString("0.00", CultureInfo.InvariantCulture)
                });
            }

            // SHIPPING
            if (productTotal < 99)
            {
                shipping = 99;

                basketItems.Add(new BasketItem
                {
                    Id = "SHIPPING",
                    Name = "Shipping Fee",
                    Category1 = "Shipping",
                    ItemType = BasketItemType.VIRTUAL.ToString(),
                    Price = shipping.ToString("0.00", CultureInfo.InvariantCulture)
                });
            }

            // ✔️ TEK DOĞRU HESAP
            decimal basketTotal = productTotal + shipping;
            decimal finalTotal = basketTotal - discount;

            // ORDER
            order.TotalPrice = finalTotal;

            // IYZICO KURALI
            request.Price = basketTotal.ToString("0.00", CultureInfo.InvariantCulture);
            request.PaidPrice = finalTotal.ToString("0.00", CultureInfo.InvariantCulture);

            request.BasketItems = basketItems;

            Payment payment = await Payment.Create(request, options);

            #endregion

            try
            {
                if (payment.Status == "success")
                {
                    await _serviceOrder.AddAsync(order);
                    var result = await _serviceOrder.SaveChangesAsync();

                    if (result > 0)
                    {
                        HttpContext.Session.Remove("Cart");
                        return RedirectToAction("Thanks");
                    }
                }
                else
                {
                        TempData["Message"] = $"<div class='alert alert-danger'>Payment failed!</div> ({payment.ErrorMessage})";
                }
              
               
            }
            catch (Exception)
            {
                TempData["Message"] = "<div class='alert alert-danger'>An error occurred!</div>";
            }
            return View(model);
        }
        private CartService GetCart()
        {
            var cart = HttpContext.Session.GetJson<CartService>("Cart");

            if (cart == null)
            {
                cart = new CartService();
                HttpContext.Session.SetJson("Cart", cart);
            }

            return cart;
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(string couponCode)
        {
            var cart = GetCart();

            if (string.IsNullOrWhiteSpace(couponCode))
            {
                TempData["Message"] = "Please enter a coupon code ❗";
                return RedirectToAction("Checkout");
            }

            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(x => x.Code == couponCode);

            if (coupon == null)
            {
                TempData["Message"] = "Coupon not found ❌";
                cart.ClearCoupon();
            }
            else if (!coupon.IsActive)
            {
                TempData["Message"] = "Coupon is not active ❌";
                cart.ClearCoupon();
            }
            else if (DateTime.Now < coupon.StartDate || DateTime.Now > coupon.EndDate)
            {
                TempData["Message"] = "Coupon expired ❌";
                cart.ClearCoupon();
            }
            else if (coupon.UsageLimit.HasValue && coupon.UsedCount >= coupon.UsageLimit)
            {
                TempData["Message"] = "Coupon limit reached ❌";
                cart.ClearCoupon();
            }
            else
            {
                cart.ApplyCoupon(coupon.Code, coupon.DiscountPercent);

                coupon.UsedCount++;
                _context.Coupons.Update(coupon);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Coupon applied ✅";
            }

            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Checkout");
        }

    }
}
