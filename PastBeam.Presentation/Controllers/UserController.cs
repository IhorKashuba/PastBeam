using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Dtos;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace PastBeam.Presentation.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpDelete("delete/{userId}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                await _userService.DeleteUserAsync(userId);
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            catch (KeyNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user.";
            }

            return RedirectToAction(nameof(UserList));
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return View(users); // Pass the list of UserListItemDto to the View
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the user list.";
                return View(new List<UserListItemDto>()); // Return empty list to avoid View error
            }
        }

        [HttpPut("suspend/{userId}")]
        public async Task<IActionResult> SuspendUser(string userId, bool isSuspended)
        {
            await _userService.SuspendUserAsync(userId, isSuspended);
            return RedirectToAction("UserList");
        }

        [HttpGet("{userId}/edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string userId)
        {
            var userDto = await _userService.GetUserForUpdateAsync(userId);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = $"User with ID {userId} not found.";
                return NotFound();
            }
            return View(userDto);
        }


        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult RegisterUserGet()
        {
            return View();
        }


        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserPost(RegisterUserDto model)
        {
            if (ModelState.IsValid)
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
            }

            return View("RegisterUserGet", model); // Назва View має збігатися з GET методом
        }


        [HttpPut("assign/{userId}/{userRole}")]
        public async Task<IActionResult> AssignUserRole(string userId, string userRole)
        {
            bool result = await _userService.AssignUserRole(userId, userRole);
            return result ? Ok() : NotFound();
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            var result = await _userService.DeleteUserAccountAsync(userId);
            return result ? Ok("Account deleted.") : NotFound();
        }

        // POST: /users/register
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterUser(RegisterUserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email); // або model.Username, залежно від реєстрації
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }


                // Якщо реєстрація не вдалася, додаємо помилки до моделі
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Якщо модель не валідна або є помилки, повертаємо на форму з помилками
            return View(model);
        }

        [HttpGet("/register")]
        [AllowAnonymous]
        public IActionResult RegisterUser()
        {
            return View("RegisterUser");
        }



    }
}

