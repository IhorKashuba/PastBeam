﻿using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Application.Library.Dtos;
using Microsoft.AspNetCore.Identity;


namespace PastBeam.Application.Library.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IBookmarkRepository _bookmarkRepository;
        private readonly IFolderRepository _folderRepository;
        private readonly IUserCourseRepository _userCourseRepository;
        private readonly Infrastructure.Library.Logger.ILogger _logger;
        private readonly UserManager<User> _userManager;

        public UserService(
            IUserRepository userRepository,
            IFavoriteRepository favoriteRepository,
            IBookmarkRepository bookmarkRepository,
            IFolderRepository folderRepository,
            IUserCourseRepository userCourseRepository,
            Infrastructure.Library.Logger.ILogger logger,
            UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _favoriteRepository = favoriteRepository;
            _bookmarkRepository = bookmarkRepository;
            _folderRepository = folderRepository;
            _userCourseRepository = userCourseRepository;
            _logger = logger;
            _userManager = userManager;
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

                _logger.LogToFile($"Successfully retrieved {userDtos.Count} users.");
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error occurred while retrieving users: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserAsync(string userId)
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
                _logger.LogToFile($"Error occurred during deletion process for user ID: {userId}. Exception: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        public async Task<Folder?> CreateFolderAsync(string userId, string folderName)
        {
            Folder folder = new Folder
            {
                UserId = userId,
                Name = folderName
            };

            await _userRepository.CreateFolderAsync(folder);

            return folder;
        }

        public async Task<IEnumerable<Folder>> GetUserFoldersAsync(string userId)
        {
            return await _userRepository.GetUserFoldersAsync(userId);
        }

        public async Task<Folder?> DeleteFolderAsync(int folderId)
        {
            return await _userRepository.DeleteFolderAsync(folderId);
        }

        public async Task<List<Article>?> GetFolderArticle(int folderId)
        {
            return await _folderRepository.GetFolderArticleAsync(folderId);
        }

        public async Task<Folder?> GetFolderAsync(int folderId)
        {
            return await _folderRepository.GetFolderAsync(folderId);
        } 

        public async Task SuspendUserAsync(string userId, bool isSuspended)
        {
            await _userRepository.SuspendUserAsync(userId, isSuspended);
            string status = isSuspended ? "suspended" : "unsuspended";
            _logger.LogToFile($"User {userId} has been {status}.");
        }

        public async Task<UserListItemDto?> GetUserAsync(string userId)
        {
            _logger.LogToFile($"Attempting to get user data for update, ID: {userId}");
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                _logger.LogToFile($"User not found for update, ID: {userId}");
                return null;
            }
            Console.WriteLine(user.UserName);

            var userDto = new UserListItemDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            _logger.LogToFile($"Successfully retrieved user data for update, ID: {userId}");
            return userDto;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDto userDto)
        {
            _logger.LogToFile($"Attempting to update user, ID: {userDto.Id}");
            var existingUser = await _userRepository.GetByIdAsync(userDto.Id);

            if (existingUser == null)
            {
                _logger.LogToFile($"User not found for update, ID: {userDto.Id}");
                return false;
            }

            existingUser.Username = userDto.Username;
            existingUser.Email = userDto.Email;
            existingUser.Role = userDto.Role;

            try
            {
                bool success = await _userRepository.UpdateUserProfileAsync(existingUser);
                if (success)
                {
                    _logger.LogToFile($"Successfully updated user, ID: {userDto.Id}");
                }
                else
                {
                    _logger.LogToFile($"Update failed for user ID: {userDto.Id} (repository returned false).");
                }
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error occurred while updating user ID: {userDto.Id}. Exception: {ex.GetType().Name} - {ex.Message}");
                throw;
            }
        }

        public async Task<bool?> UpdateUserProfileAsync(string userId, string? username = null, string? email = null, string? passwordHash = null)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return null;

            if (username != null) user.Username = username;
            if (email != null) user.Email = email;
            if (passwordHash != null) user.PasswordHash = passwordHash;

            try
            {
                await _userRepository.UpdateUserProfileAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error while updating user profile {ex}");
                throw;
            }
        }

        public async Task<bool> AssignUserRole(string userId, string userRole)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            user.Role = userRole;
            return await _userRepository.UpdateUserProfileAsync(user);
        }

        public async Task<bool> DeleteUserAccountAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            await _favoriteRepository.DeleteFavoritesByUserAsync(userId);
            await _bookmarkRepository.DeleteBookmarksByUserAsync(userId);
            await _folderRepository.DeleteFoldersByUserAsync(userId);
            await _userCourseRepository.DeleteUserCoursesByUserAsync(userId);
            await _userRepository.DeleteUserAsync(userId);

            _logger.LogToFile($"User {userId} has deleted their account.");
            return true;
        }


        // test password hashing
        public async Task<IdentityResult> RegisterUserAsync(RegisterUserDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Користувач з такою поштою вже існує."
                });
            }

            var user = new User
            {
                UserName = model.Username, // або model.Email, якщо логін за email
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            return result;
        }

    }
}
