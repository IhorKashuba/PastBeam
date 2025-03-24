using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Presentation.Controllers
{
    public class UserController
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

        public async Task<IEnumerable<Folder>> GetUserFolders(int userId)
        {
            return await _userService.GetUserFoldersAsync(userId);
        }

    }
}
