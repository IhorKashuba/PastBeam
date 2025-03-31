using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Presentation.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
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

        [HttpGet]
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

        public async Task CreateFolder(int userId, string folderName)
        {
            await _userService.CreateFolderAsync(userId, folderName);
        }

        public async Task DeleteFolder(int folderId)
        {
            await _userService.DeleteFolderAsync(folderId);
        }

        public async Task<IActionResult> GetUserFolders(int userId)
        {
            var folders = await _userService.GetUserFoldersAsync(userId);
            return View("FolderList",folders);
        }

        public async Task AssignUserRole(int userId, string userRole)
        {
            bool result = await _userService.AssignUserRole(userId, userRole);

        }

    }
}
