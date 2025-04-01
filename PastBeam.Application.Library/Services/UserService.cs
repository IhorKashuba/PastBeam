using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Application.Library.Dtos;


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

        public async Task<IEnumerable<UserListItemDto>> GetAllUsersAsync()
        {
            _logger.LogToFile("Attempting to retrieve all users.");
            try
            {
                var users = await _userRepository.GetAllAsync();

                var userDtos = users.Select(user => new UserListItemDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                }).ToList();

                _logger.LogToFile($"Successfully retrieved {userDtos.Count()} users.");
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error occurred while retrieving users: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            _logger.LogToFile($"Attempting to delete user with ID: {userId}");

            var userExists = await _userRepository.GetByIdAsync(userId);
            if (userExists == null)
            {
                _logger.LogToFile($"Warning: User with ID: {userId} not found for deletion.");
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }


            try
            {
                await _userRepository.DeleteAsync(userId);
                _logger.LogToFile($"Successfully initiated deletion for user with ID: {userId}.");
            }
            catch (Exception ex)
            {
                // Combine error message and exception details into one string for LogToFile
                _logger.LogToFile($"Error occurred during deletion process for user ID: {userId}. Exception: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

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


        public async Task<IEnumerable<Folder>> GetUserFoldersAsync(int userId)
        {
            return await _userRepository.GetUserFoldersAsync(userId);
        }


        public Task<Folder?> DeleteFolderAsync(int folderId)
        {
            return _userRepository.DeleteFolderAsync(folderId);
        }

        public async Task SuspendUserAsync(int userId, bool isSuspended)
        {
            await _userRepository.SuspendUserAsync(userId, isSuspended);
            string status = isSuspended ? "suspended" : "unsuspended";
            _logger.LogToFile($"User {userId} has been {status}.");
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
