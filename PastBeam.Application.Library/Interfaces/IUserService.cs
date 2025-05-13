using PastBeam.Core.Library.Entities;
using PastBeam.Application.Library.Dtos;

namespace PastBeam.Application.Library.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserListItemDto>> GetAllUsersAsync();
        Task DeleteUserAsync(string userId);

        Task<Folder?> CreateFolderAsync(string userId, string name);

        //temporary userId because user will be saved in session
        Task<IEnumerable<Folder>> GetUserFoldersAsync(string userId);

        Task<Folder?> DeleteFolderAsync(int folderId);

        Task SuspendUserAsync(string userId, bool isSuspended);
        
        Task<UserListItemDto?> GetUserAsync(string userId); // For loading edit form
        
        Task<bool> UpdateUserAsync(UpdateUserDto userDto);

        Task<bool?> UpdateUserProfileAsync(string userId, string? username = null, string? email = null, string? passwordHash = null);

        Task<bool> AssignUserRole(string userId, string userRole);

        Task<bool> DeleteUserAccountAsync(string userId);


    }
}
