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
            return await _userRepository.GetUserFoldersAsync(userId); ;
        }


        public Task<Folder?> DeleteFolderAsync(int folderId)
        {
            return _userRepository.DeleteFolderAsync(folderId);
        }
    }
}
