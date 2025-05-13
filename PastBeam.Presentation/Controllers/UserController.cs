using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Dtos;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using System.Security.Claims;

namespace PastBeam.Presentation.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private readonly UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public UserController(IUserService userService, UserManager<User> userManager, SignInManager<User> singInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = singInManager;
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
                // Return the view without data, or redirect to an error page
                return View(new List<PastBeam.Application.Library.Dtos.UserListItemDto>()); // Return empty list to avoid View error
                // Or return View("Error", new ErrorViewModel { /* ... */ });
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
            if (!ModelState.IsValid){
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
    }
}
