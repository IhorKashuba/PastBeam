using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using System.Security.Claims;

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

        [HttpPost("Create")]
        public async Task<IActionResult> CreateFolder([FromForm] string folderName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _userService.CreateFolderAsync(userId, folderName);

            return RedirectToAction("GetUserFolders", "Folder");
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteFolder(int folderId)
        {
            var succes = await _userService.DeleteFolderAsync(folderId);

            if (succes == null)
            {
                TempData["ErrorMessage"] = "Error occured while deleting folder.";
                return RedirectToAction("GetUserFolders", "Folder");
            }
            return RedirectToAction("GetUserFolders", "Folder");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetUserFolders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var folders = await _userService.GetUserFoldersAsync(userId);
            ViewBag.folders = folders;
            return View("~/Views/User/Folder/FolderList.cshtml", folders);
        }

        [HttpGet]
        public async Task<IActionResult> GetFolderArticles(int folderId)
        {
            List<Article>? articles = await _userService.GetFolderArticle(folderId);

            Folder folder = await _userService.GetFolderAsync(folderId);

            ViewBag.folder = folder;
            return View("~/Views/User/Folder/FolderPage.cshtml", articles);
        }
    }
}
