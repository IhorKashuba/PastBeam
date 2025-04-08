using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Dtos;
using PastBeam.Application.Library.Interfaces;

namespace PastBeam.Presentation.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpDelete("delete/{userId}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int userId)
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
        public async Task<IActionResult> SuspendUser(int userId, bool isSuspended)
        {
            await _userService.SuspendUserAsync(userId, isSuspended);
            return RedirectToAction("UserList");
        }

        [HttpGet("{id:int}/edit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(int id)
        {
            var userDto = await _userService.GetUserForUpdateAsync(id);
            if (userDto == null)
            {
                TempData["ErrorMessage"] = $"User with ID {id} not found.";
                return NotFound();
            }
            return View(userDto);
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(UpdateUserDto userDto)
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

        [HttpPut("assign/{userId}/{userRole}")]
        public async Task AssignUserRole(int userId, string userRole)
        {
            bool result = await _userService.AssignUserRole(userId, userRole);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await _userService.DeleteUserAccountAsync(id);
            return result ? Ok("Account deleted.") : NotFound();
        }
    }
}
