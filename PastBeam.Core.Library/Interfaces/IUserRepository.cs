using Microsoft.AspNetCore.Identity;
using PastBeam.Core.Library.Entities;


namespace PastBeam.Core.Library.Interfaces
{
    public interface IUserRepository
    {
        // User related methods
        Task<User?> GetByIdAsync(string userId);

        Task<IEnumerable<User>> GetAllAsync();

        Task DeleteAsync(string userId);


        // Folder related methods
        Task<IEnumerable<Folder>> GetUserFoldersAsync(string userId);

        Task<Folder?> DeleteFolderAsync(int folderId);

        Task CreateFolderAsync(Folder folder);

        Folder? GetFolderById(int folderId);

        Task SuspendUserAsync(string userId, bool isSuspended);
      
        Task<User?> GetUserByIdAsync(string userId);

        Task<bool> UpdateUserProfileAsync(User user);

        Task DeleteUserAsync(string userId);

        Task<User?> GetByEmailAsync(string email);

    }
}
