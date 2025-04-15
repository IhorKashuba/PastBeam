using PastBeam.Core.Library.Entities;
using PastBeam.Application.Library.Dtos;


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

        Task SuspendUserAsync(int userId, bool isSuspended);
        
        Task<UpdateUserDto?> GetUserForUpdateAsync(int userId); // For loading edit form
        
        Task<bool> UpdateUserAsync(UpdateUserDto userDto);

        Task<User?> UpdateUserProfileAsync(int userId, string? username = null, string? email = null, string? passwordHash = null);

        Task<bool> AssignUserRole(int userId, string userRole);

        Task<bool> DeleteUserAccountAsync(int userId);

        Task SendPasswordResetEmailAsync(string email);
        Task<(bool Succeeded, List<string> Errors)> ResetPasswordAsync(ResetPasswordDto model);
    }
}
