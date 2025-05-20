using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Dtos;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Security.Claims;
using PastBeam.Presentation.Models;


namespace PastBeam.Presentation.Controllers
{
    [Route("user")]
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

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> ShowProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                TempData["ErrorMessage"] = "User is not authenticated.";
                return RedirectToAction("Login", "Account");
            }

            var userDto = await _userService.GetUserAsync(userId);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = $"User with ID {userId} not found.";
                return NotFound();
            }

            ViewBag.User = userDto;

            return View("UserPage", userDto);
        }

        [HttpGet("{userId}/edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string userId)
        {
            var userDto = await _userService.GetUserAsync(userId);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = $"User with ID {userId} not found.";
                return NotFound();
            }
            return View(userDto);
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserAdmin(UpdateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            try
            {
                bool success = await _userService.UpdateUserAsync(userDto);
                if (success)
                {
                    TempData["SuccessMessage"] = $"User '{userDto.Username}' (ID: {userDto.Id}) updated successfully.";
                    return RedirectToAction(nameof(UserList));
                }
                else
                {
                    TempData["ErrorMessage"] = $"Could not update user with ID {userDto.Id}. User might not exist anymore.";
                    return View(userDto);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating the user.";
                return View(userDto);
            }
        }

        [HttpPost("/edit/username")]
        public async Task<IActionResult> UpdateUsername(string NewUsername)
        {
            var user = await _userManager.GetUserAsync(User); // Отримуємо поточного користувача
            if (user == null) return NotFound();

            try
            {
                var success = await _userService.UpdateUserProfileAsync(user.Id, username: NewUsername); // ← Виклик вашого методу

                if (success != true)
                {
                    TempData["Error"] = "Failed to update username.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while updating username.";
                // За потреби логування або обробка
            }

            return RedirectToAction("ShowProfile");
        }

        [HttpPost("/edit/password")]
        public async Task<IActionResult> UpdatePassword(string CurrentPassword, string NewPassword, string ConfirmPassword)
        {
            if (NewPassword != ConfirmPassword)
            {
                TempData["Error"] = "Passwords do not match.";
                return RedirectToAction("ShowProfile");
            }

            var user = await _userManager.GetUserAsync(User); // Отримуємо поточного користувача
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, CurrentPassword, NewPassword);

            if (!result.Succeeded)
            {
                TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Description));
                return RedirectToAction("ShowProfile");
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("ShowProfile");
        }

        [HttpPut("assign/{userId}/{userRole}")]
        [Authorize(Roles = "Admin")]
        public async Task AssignUserRole(string userId, string userRole)
        {
            bool result = await _userService.AssignUserRole(userId, userRole);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            var result = await _userService.DeleteUserAccountAsync(userId);
            return result ? Ok("Account deleted.") : NotFound();
        }

        // GET: /user/forgot-password
        [HttpGet("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /user/forgot-password
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Не показуємо, що користувача немає
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "User", new { code, email = model.Email }, protocol: Request.Scheme);

            // TODO: Надіслати email
            Console.WriteLine($"Send this reset link via email: {callbackUrl}");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        // GET: /user/forgot-password-confirmation
        [HttpGet("forgot-password-confirmation")]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /user/reset-password
        [HttpGet("reset-password")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string email = null)
        {
            if (code == null || email == null)
            {
                return BadRequest("A code and email must be supplied.");
            }
            return View(new ResetPasswordViewModel { Code = code, Email = email });
        }

        // POST: /user/reset-password
        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // GET: /user/reset-password-confirmation
        [HttpGet("reset-password-confirmation")]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}
