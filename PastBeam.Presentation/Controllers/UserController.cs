using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPut("assign/{userId}/{userRole}")]
        public async Task AssignUserRole(int userId, string userRole)
        {
            bool result = await _userService.AssignUserRole(userId, userRole);

        }
    }
}
