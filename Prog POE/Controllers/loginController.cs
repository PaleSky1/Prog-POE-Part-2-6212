using Microsoft.AspNetCore.Mvc;
using Prog_POE.Models;
using Prog_POE.Services;
using System.Threading.Tasks;

namespace Prog_POE.Controllers
{
    public class LoginController : Controller
    {
        private readonly TableStorageService _tableStorageService;

        public LoginController(TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _tableStorageService.GetUserAsync(model.Username);
                if (user != null && user.Password == model.Password)
                {
                    HttpContext.Session.SetString("Username", user.RowKey);
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _tableStorageService.GetUserAsync(model.Username);
                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        PartitionKey = "User",
                        RowKey = model.Username,
                        Password = model.Password
                    };

                    await _tableStorageService.AddUserAsync(newUser);
                    return RedirectToAction("Index", "Login");
                }
                ModelState.AddModelError(string.Empty, "Username already exists.");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Dashboard", "Home");
        }
    }
}
