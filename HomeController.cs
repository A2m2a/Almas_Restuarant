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
using System.Security.Claims;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace آخرین_الماس.Controllers
{
    public class HomeController : Controller
    {

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
            if (user == null || string.IsNullOrWhiteSpace(user.Password) || string.IsNullOrWhiteSpace(user.Username))
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
                TempData["Success"] = "خوش آمدید";

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
        public IActionResult MENUASLI()
        {
            return View();

            //    var foods = _context.Foods.ToList();
            //    return View(foods);
        }
        public IActionResult PASSMENU()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult AddToCart([FromBody]CartItem model)
        //{
        //    int userId = User.Identity.IsAuthenticated
        //                 ? int.Parse(User.Identity.Name)
        //                 : Math.Abs(HttpContext.Session.Id.GetHashCode());

        //    var existing = _context.CartItems
        //        .FirstOrDefault(c => c.UserId == userId && c.FoodId == model.FoodId);

        //    // اینجا مطمئن شو که Food رو از جدول Foods میگیری
        //    var food = _context.Foods.FirstOrDefault(f => f.Id == model.FoodId);
        //    if (food == null)
        //        return Json(new { success = false, message = "غذا پیدا نشد" });

        //    if (existing != null)
        //    {
        //        existing.Quantity += model.Quantity;
        //        _context.Update(existing);
        //    }
        //    else
        //    {
        //        model.UserId = userId;
        //        model.ProductName = food.Name;
        //        model.Price = food.Price;
        //        _context.Add(model);
        //    }

        //    _context.SaveChanges();
        //    return Json(new { success = true });
        //}
        [HttpPost]
        public IActionResult AddToCart(CartItem model)
        {
            int userId = User.Identity.IsAuthenticated
                    ? int.Parse(User.Identity.Name)
                    : Math.Abs(HttpContext.Session.Id.GetHashCode());

            // گرفتن اطلاعات غذا
            var food = _context.Foods.FirstOrDefault(f => f.Id == model.FoodId);
            if (food == null)
                return Json(new { success = false, message = "غذا پیدا نشد" });

            var existing = _context.CartItems
                .FirstOrDefault(c => c.UserId == userId && c.FoodId == model.FoodId);

            if (existing != null)
            {
                existing.Quantity += model.Quantity;
                _context.Update(existing);
            }
            else
            {
                model.UserId = userId;
                model.ProductName = food.Name;  // نام غذا از جدول Foods
                model.Price = food.Price;       // قیمت غذا از جدول Foods
                _context.Add(model);
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }
        public IActionResult GetCart()
        {
            int userId;

            if (User.Identity.IsAuthenticated)
            {
                userId = int.Parse(User.Identity.Name);
            }
            else
            {
                userId = Math.Abs(HttpContext.Session.Id.GetHashCode());
            }

            var items = _context.CartItems
                .Where(c => c.UserId == userId)
                .Select(c => new { c.Id, c.FoodId, c.ProductName, Quantity=(int)c.Quantity, Total = c.Price * c.Quantity })
                .ToList();

            return Json(new { items });
        }
        public IActionResult MENU()
        {
            var foods = _context.Foods.ToList();
            return View(foods);
        }
       
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // یا await SignOutAsync(); اگر از Identity استفاده می‌کنی
            return RedirectToAction("LOG1");
        }
     
        [HttpGet]
        public IActionResult NAZAR()
        {
            var feedbacks = _context.Feedbacks
                             //.OrderByDescending(f => f.CreatedAt)
                             .ToList();

            var model = new HomeViewModel
            {
                Feedbacks = feedbacks
            };

            return View(model);
        }
        public IActionResult Feedback()
        {
            var model = new HomeViewModel
            {
                Feedbacks = _context.Feedbacks
                    .OrderByDescending(f => f.CreatedAt)
                    .ToList()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitFeedback(string Message, int Rating)
        {
            System.Diagnostics.Debug.WriteLine("Received Rating: " + Rating);
            var username = HttpContext.Session.GetString("Username");

            if (!string.IsNullOrEmpty(username))
            {
                var feedback = new Feedback
                {
                    Username = username,
                    Message = Message,
                    Rating = Rating,
                    CreatedAt = DateTime.Now
                };

                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();

                return RedirectToAction(nameof(NAZAR));
            }
            else
            {
                TempData["Error"] = "برای ارسال نظر ابتدا وارد شوید.";
                return RedirectToAction("LOG1");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
