using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RabbitMQWeb.ExcelCreate.Models;

namespace RabbitMQWeb.ExcelCreate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Email,string Password)
        {
            var user=await _userManager.FindByEmailAsync(Email);
            if (user == null) throw new Exception("user is not found");
            var userPasswordCheck =await _userManager.CheckPasswordAsync(user, Password);
            if (userPasswordCheck == false) throw new Exception("Email or Password is incorrect");
            var signInResult=await _signInManager.PasswordSignInAsync(user, Password,true,false);
            return RedirectToAction("Index","Home");
        }
    }
}
