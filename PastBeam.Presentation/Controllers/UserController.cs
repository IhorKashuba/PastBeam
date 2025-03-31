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

        [HttpPost]
        public async Task<IActionResult> SuspendUser(int userId, bool isSuspended)
        {
            await _userService.SuspendUserAsync(userId, isSuspended);
            return RedirectToAction("UserList");
        }
    }
}
