using PastBeam.Core.Library.Entities;
using PastBeam.Application.Library.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserListItemDto>> GetAllUsersAsync();
        Task DeleteUserAsync(int userId);

        Task<Folder?> CreateFolderAsync(int userId, string name);

        //temporary userId because user will be saved in session
        Task<IEnumerable<Folder>> GetUserFoldersAsync(int userId);

        Task<Folder?> DeleteFolderAsync(int folderId);

        Task<User?> UpdateUserProfileAsync(int userId, string? username = null, string? email = null, string? passwordHash = null);

        Task<bool> AssignUserRole(int userId, string userRole);
    }
}
