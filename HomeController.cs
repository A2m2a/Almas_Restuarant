using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using آخرین_الماس.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace آخرین_الماس.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult LOG1(User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Password)||  string.IsNullOrWhiteSpace(user.Username))
{
                ViewBag.Error = "اطلاعات وارد شده ناقص است.";
                return View("LOG1");
            }
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("LOG2"); // یا هر صفحه بعدی
        }
        [HttpGet]
        public IActionResult LOG2()
        {
            return View();
        }
        [HttpGet]
        public IActionResult LOG3()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PICTURES()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == Username);
            if (user == null)
            {
                TempData["Error"] = "کاربری با این نام وجود ندارد. لطفاً ثبت‌نام کنید.";
                return RedirectToAction("LOG1"); // هدایت به صفحه ثبت‌نام
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, Password);

            if (result == PasswordVerificationResult.Success)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.Id);

                // برو به اکشن Pictures در کنترلر جاری (مثلاً HomeController)
                TempData["Success"] ="خوش آمدید";

                return RedirectToAction("Pictures");
            }
            else
            {
                TempData["Error"] = "رمز عبور اشتباه است.";
                return RedirectToAction("LOG1");
            }
        }
        [HttpPost]
        public IActionResult LOG2(string username, string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                TempData["Error"] = "این نام کاربری قبلاً ثبت شده است.";
                return RedirectToAction("LOG2");
            }

            var hasher = new PasswordHasher<User>();
            var newUser = new User
            {
                Username = username,
                PasswordHash = hasher.HashPassword(null, password)
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetString("Username", newUser.Username);
            HttpContext.Session.SetInt32("UserId", newUser.Id);

            return RedirectToAction("Pictures");
        }
        [HttpPost]
        public IActionResult LOG3(string username, string newPassword)
        {
            // بررسی وجود کاربر
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                TempData["Error"] = "کاربری با این نام یافت نشد.";
                return RedirectToAction("LOG3");
            }

            // هش کردن رمز عبور جدید
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);

            // ذخیره تغییرات
            _context.SaveChanges();

            TempData["Success"] = "رمز عبور با موفقیت تغییر یافت.";
            return RedirectToAction("LOG1"); // برگشت به صفحه ورود
        }
        public IActionResult MENU()
        {
            return View();
        }
        public IActionResult MENUASLI()
        {
            return View();
        }
        public IActionResult PASSMENU()
        {
            return View();
        }
        public IActionResult NAZAR()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddToCart(int foodId)
        {
      var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "برای افزودن به سبد خرید، باید وارد حساب شوید.";
                return RedirectToAction("Login", "Account");
            }

            // پیدا کردن غذا
            var food = _context.Foods.FirstOrDefault(f => f.Id == foodId);
            if (food == null)
            {
                TempData["error"] = "غذا یافت نشد.";
                return RedirectToAction("MENURESULT");
            }

            // بررسی آیا غذا قبلاً در سبد بوده یا نه
            var cartItem = _context.CartItems
                .FirstOrDefault(c => c.FoodId == food.Id && c.UserId == userId);

            if (cartItem != null)
            {
                cartItem.Quantity += 1; // افزایش تعداد
            }
            else
            {
                cartItem = new CartItem
                {
                    FoodId = food.Id,
                    FoodName = food.Name,
                    Quantity = 1,
                    Price = food.Price,
                    UserId = userId
                };

                _context.CartItems.Add(cartItem); // افزودن به دیتابیس
            }

            _context.SaveChanges(); // ذخیره

            TempData["success"] = "غذا به سبد خرید اضافه شد.";
            return RedirectToAction("MENUASLI");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
