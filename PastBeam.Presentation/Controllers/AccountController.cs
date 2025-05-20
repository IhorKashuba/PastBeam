using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PastBeam.Core.Library.Entities; // AppUser
using PastBeam.Presentation.Models;
using Microsoft.AspNetCore.Authorization; // LoginViewModel
using PastBeam.Application.Library.Dtos;
using PastBeam.Application.Library.Interfaces;

namespace PastBeam.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager,IUserService userService, UserManager<User> userManager )
        {
            _signInManager = signInManager;
            _userService = userService;
            _userManager = userManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Account/Login.cshtml");
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неправильний логін або пароль");
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("register")]
        public IActionResult RegisterUser()
        {
            return View("RegisterUser");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserDto model)
        {

            var result = await _userService.RegisterUserAsync(model);


            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("RegisterUser", model);
        }

    }
}
