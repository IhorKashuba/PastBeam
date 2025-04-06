using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Interfaces;

namespace PastBeam.Presentation.Controllers
{
    [Route("folders")]
    public class FolderController : Controller
    {
        private IUserService _userService;

        public FolderController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create/{folderName}")]
        public async Task CreateFolder(int userId, string folderName)
        {
            await _userService.CreateFolderAsync(userId, folderName);
        }

        [HttpDelete("delete/{folderId}")]
        public async Task DeleteFolder(int folderId)
        {
            await _userService.DeleteFolderAsync(folderId);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUserFolders(int userId)
        {
            var folders = await _userService.GetUserFoldersAsync(userId);
            return View("FolderList", folders);
        }
    }
}
