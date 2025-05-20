using Microsoft.AspNetCore.Identity;
using Moq;
using PastBeam.Application.Library.Dtos;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Logger;
using Xunit;


public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IFavoriteRepository> _favoriteRepositoryMock;
    private readonly Mock<IBookmarkRepository> _bookmarkRepositoryMock;
    private readonly Mock<IFolderRepository> _folderRepositoryMock;
    private readonly Mock<IUserCourseRepository> _userCourseRepositoryMock;
    private readonly Mock<ILogger> _mockLogger;
    private readonly UserService _userService;
    private readonly Mock<UserManager<User>> _userManagerMock;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger>();
        _favoriteRepositoryMock = new Mock<IFavoriteRepository>();
        _bookmarkRepositoryMock = new Mock<IBookmarkRepository>();
        _folderRepositoryMock = new Mock<IFolderRepository>();
        _userCourseRepositoryMock = new Mock<IUserCourseRepository>();
        _userManagerMock = GetMockUserManager();

        _userService = new UserService(
            _mockUserRepository.Object,
            _favoriteRepositoryMock.Object,
            _bookmarkRepositoryMock.Object,
            _folderRepositoryMock.Object,
            _userCourseRepositoryMock.Object,
            _mockLogger.Object,
            _userManagerMock.Object);
    }

    private Mock<UserManager<User>> GetMockUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object,
            null, null, null, null, null, null, null, null);
    }


    [Fact]
    public async Task GetAllUsersAsync_ReturnsMappedDtos()
    {
        // Arrange
        var usersFromRepo = new List<User>
        {
            new User { Id = "test_id_1", Username = "AdminUser", Email = "admin@test.com", Role = "Admin", CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new User { Id = "test_id_2", Username = "RegularUser", Email = "user@test.com", Role = "User", CreatedAt = DateTime.UtcNow.AddDays(-5) }
        };
        _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(usersFromRepo);

        // Act
        var result = await _userService.GetAllUsersAsync();
        var resultList = result.ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, resultList.Count);

        Assert.Equal(usersFromRepo[0].Id, resultList[0].Id);
        Assert.Equal(usersFromRepo[0].Username, resultList[0].Username);
        Assert.Equal(usersFromRepo[0].Email, resultList[0].Email);
        Assert.Equal(usersFromRepo[0].Role, resultList[0].Role);
        Assert.Equal(usersFromRepo[0].CreatedAt, resultList[0].CreatedAt);
        // PasswordHash should NOT be present in the DTO

        Assert.Equal(usersFromRepo[1].Id, resultList[1].Id);
        Assert.Equal(usersFromRepo[1].Username, resultList[1].Username);
        Assert.Equal(usersFromRepo[1].Email, resultList[1].Email);
        Assert.Equal(usersFromRepo[1].Role, resultList[1].Role);
        Assert.Equal(usersFromRepo[1].CreatedAt, resultList[1].CreatedAt);

        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Attempting to retrieve all users"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Successfully retrieved 2 users"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error occurred"))), Times.Never);
    }

    [Fact]
    public async Task GetAllUsersAsync_RepositoryReturnsEmpty_ReturnsEmptyDtoList()
    {
        // Arrange
        var emptyUserList = new List<User>();
        _mockUserRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(emptyUserList);

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Attempting to retrieve all users"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Successfully retrieved 0 users"))), Times.Once); // Count is 0
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error occurred"))), Times.Never);
    }

    [Fact]
    public async Task GetAllUsersAsync_RepositoryThrowsException_RethrowsAndLogsError()
    {
        // Arrange
        var repositoryException = new InvalidOperationException("Database access error");
        _mockUserRepository.Setup(repo => repo.GetAllAsync()).ThrowsAsync(repositoryException);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.GetAllUsersAsync());

        Assert.Same(repositoryException, actualException); // Verify the original exception is rethrown
        _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Attempting to retrieve all users"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error occurred while retrieving users"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Successfully retrieved"))), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_UserExists_CallsRepositoryDeleteAsync()
    {
        // Arrange
        var userId = "testid";
        var userToDelete = new User { Id = userId, Username = "TestUser" };
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                           .ReturnsAsync(userToDelete);
        _mockUserRepository.Setup(repo => repo.DeleteAsync(userId))
                           .Returns(Task.CompletedTask);

        // Act
        await _userService.DeleteUserAsync(userId);

        // Assert
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Attempting to delete"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Successfully initiated deletion"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Warning:"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error occurred"))), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_UserNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = "testid";
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                           .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteUserAsync(userId));

        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.DeleteAsync(It.IsAny<string>()), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Attempting to delete"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Warning: User with ID"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Successfully initiated deletion"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error occurred"))), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_RepositoryThrowsException_RethrowsAndLogsError()
    {
        // Arrange
        var userId = "testid";
        var userToDelete = new User { Id = userId, Username = "TestUser" };
        var repositoryException = new InvalidOperationException("Database error");

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                           .ReturnsAsync(userToDelete);
        _mockUserRepository.Setup(repo => repo.DeleteAsync(userId))
                           .ThrowsAsync(repositoryException);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.DeleteUserAsync(userId));

        Assert.Same(repositoryException, actualException);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Attempting to delete"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error occurred during deletion"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Warning:"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Successfully initiated deletion"))), Times.Never);
    }

    [Fact]
    public async Task GetUserForUpdateAsync_UserExists_ReturnsCorrectDto()
    {
        string userId = "testid";
        var userEntity = new User { Id = userId, Username = "ExistingUser", Email = "exist@test.com", Role = "User" };
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(userEntity);

        var resultDto = await _userService.GetUserAsync(userId);

        Assert.NotNull(resultDto);
        Assert.Equal(userEntity.Id, resultDto.Id);
        Assert.Equal(userEntity.Username, resultDto.Username);
        Assert.Equal(userEntity.Email, resultDto.Email);
        Assert.Equal(userEntity.Role, resultDto.Role);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Attempting to get user data for update, ID: {userId}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Successfully retrieved user data for update, ID: {userId}"))), Times.Once);
    }

    [Fact]
    public async Task GetUserForUpdateAsync_UserNotFound_ReturnsNull()
    {
        string userId = "testid";
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        var resultDto = await _userService.GetUserAsync(userId);

        Assert.Null(resultDto);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"User not found for update, ID: {userId}"))), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_UserExists_UpdatesPropertiesAndCallsRepository_ReturnsTrue()
    {
        string userId = "testid";
        var updateUserDto = new UpdateUserDto { Id = userId, Username = "UpdatedName", Email = "updated@test.com", Role = "Admin" };
        var existingUserEntity = new User { Id = userId, Username = "OldName", Email = "old@test.com", Role = "User" };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUserEntity);
        _mockUserRepository.Setup(repo => repo.UpdateUserProfileAsync(It.IsAny<User>()))
                           .ReturnsAsync(true);

        var result = await _userService.UpdateUserAsync(updateUserDto);

        Assert.True(result);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.UpdateUserProfileAsync(It.Is<User>(u =>
            u.Id == userId &&
            u.Username == updateUserDto.Username &&
            u.Email == updateUserDto.Email &&
            u.Role == updateUserDto.Role
        )), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Attempting to update user, ID: {userId}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Successfully updated user, ID: {userId}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("User not found"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Update failed"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error occurred"))), Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_UserNotFound_ReturnsFalse()
    {
        string userId = "testid";
        var updateUserDto = new UpdateUserDto { Id = userId, Username = "NonExistent", Email = "a@b.com", Role = "User" };
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync((User?)null);

        var result = await _userService.UpdateUserAsync(updateUserDto);

        Assert.False(result);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.UpdateUserProfileAsync(It.IsAny<User>()), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"User not found for update, ID: {userId}"))), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_RepositoryUpdateFails_ReturnsFalse()
    {
        string userId = "testid";
        var updateUserDto = new UpdateUserDto { Id = userId, Username = "UpdateFail", Email = "fail@test.com", Role = "User" };
        var existingUserEntity = new User { Id = userId, Username = "Original", Email = "orig@test.com", Role = "User" };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUserEntity);
        _mockUserRepository.Setup(repo => repo.UpdateUserProfileAsync(It.IsAny<User>()))
                           .ReturnsAsync(false);

        var result = await _userService.UpdateUserAsync(updateUserDto);

        Assert.False(result);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.UpdateUserProfileAsync(It.IsAny<User>()), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Update failed for user ID: {userId}"))), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_RepositoryThrowsException_RethrowsAndLogsError()
    {
        string userId = "testid";
        var updateUserDto = new UpdateUserDto { Id = userId, Username = "ExceptionUser", Email = "ex@test.com", Role = "User" };
        var existingUserEntity = new User { Id = userId, Username = "OrigEx", Email = "origex@test.com", Role = "User" };
        var repositoryException = new Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException("Concurrency conflict"); // Use specific exception if known

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId)).ReturnsAsync(existingUserEntity);
        _mockUserRepository.Setup(repo => repo.UpdateUserProfileAsync(It.IsAny<User>()))
                           .ThrowsAsync(repositoryException);

        var actualException = await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException>(() => _userService.UpdateUserAsync(updateUserDto));

        Assert.Same(repositoryException, actualException);
        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.UpdateUserProfileAsync(It.IsAny<User>()), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Error occurred while updating user ID: {userId}"))), Times.Once);
    }

    [Fact]
    public async Task AssignUserRole_ShouldUpdateUserRole_WhenUserExists()
    {
        string userId = "testid";
        string newRole = "Admin";
        var user = new User { Id = userId, Role = "User" };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

        _mockUserRepository.Setup(repo => repo.UpdateUserProfileAsync(It.IsAny<User>())).ReturnsAsync(true);

        bool result = await _userService.AssignUserRole(userId, newRole);

        Assert.True(result);
        Assert.Equal(newRole, user.Role);

        _mockUserRepository.Verify(repo => repo.UpdateUserProfileAsync(It.Is<User>(u => u.Id == userId && u.Role == newRole)), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAccountAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(r => r.GetUserByIdAsync(It.IsAny<string>()))
                    .ReturnsAsync(new User { Id = "test_id_1" });

            mockRepo.Setup(r => r.DeleteUserAsync(It.IsAny<string>()))
                    .Returns(Task.CompletedTask);

            var service = new UserService(mockRepo.Object, _favoriteRepositoryMock.Object, _bookmarkRepositoryMock.Object, _folderRepositoryMock.Object, _userCourseRepositoryMock.Object, _mockLogger.Object, _userManagerMock.Object);

            // Act
            var result = await service.DeleteUserAccountAsync("test_id_1");

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.DeleteUserAsync("test_id_1"), Times.Once);
        }

}