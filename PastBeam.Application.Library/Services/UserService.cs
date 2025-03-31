using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;


namespace PastBeam.Application.Library.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private Infrastructure.Library.Logger.ILogger _logger;

        public UserService(IUserRepository userRepository, Infrastructure.Library.Logger.ILogger logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        //Create folder
        public async Task<Folder?> CreateFolderAsync(int userId, string folderName)
        {
            Folder folder = new Folder
            {
                UserId = userId,
                Name = folderName
            };

            await _userRepository.CreateFolderAsync(folder);

            return folder;
        }

        //Get all folders for specific user
        public async Task<IEnumerable<Folder>> GetUserFoldersAsync(int userId)
        {
            return await _userRepository.GetUserFoldersAsync(userId); ;
        }

        //Delete folders
        public Task<Folder?> DeleteFolderAsync(int folderId)
        {
            return _userRepository.DeleteFolderAsync(folderId);
        }

        public async Task<User?> UpdateUserProfileAsync(int userId, string? username = null, string? email = null, string? passwordHash = null)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return null;

            if (username != null)
            {
                user.Username = username;
            }
            if (email != null)
            {
                user.Email = email;
            }
            if (passwordHash != null)
            {
                user.PasswordHash = passwordHash;
            }

            try
            {
                await _userRepository.UpdateUserProfileAsync(user);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error while updating user profile {ex}");
                throw;
            }
        }

        public async Task<bool> AssignUserRole(int userId, string userRole)
        {
            User user = await _userRepository.GetUserByIdAsync(userId);

            user.Role = userRole;

            return await _userRepository.UpdateUserProfileAsync(user);
        }

    }
}
