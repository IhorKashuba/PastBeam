using Moq;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Logger;
using Xunit;


public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger>();
        _userService = new UserService(_mockUserRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsMappedDtos()
    {
        // Arrange
        var usersFromRepo = new List<User>
        {
            new User { Id = 1, Username = "AdminUser", Email = "admin@test.com", Role = "Admin", CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new User { Id = 2, Username = "RegularUser", Email = "user@test.com", Role = "User", CreatedAt = DateTime.UtcNow.AddDays(-5) }
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
        var userId = 1;
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
        var userId = 1;
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                           .ReturnsAsync((User?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteUserAsync(userId));

        _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        _mockUserRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Attempting to delete"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Warning: User with ID"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Successfully initiated deletion"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error occurred"))), Times.Never);
    }

    [Fact]
    public async Task DeleteUserAsync_RepositoryThrowsException_RethrowsAndLogsError()
    {
        // Arrange
        var userId = 1;
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
    public async Task AssignUserRole_ShouldUpdateUserRole_WhenUserExists()
    {
        int userId = 1;
        string newRole = "Admin";
        var user = new User { Id = userId, Role = "User" };

        _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(user);

        _mockUserRepository.Setup(repo => repo.UpdateUserProfileAsync(It.IsAny<User>())).ReturnsAsync(true);

        bool result = await _userService.AssignUserRole(userId, newRole);

        Assert.True(result);
        Assert.Equal(newRole, user.Role);

        _mockUserRepository.Verify(repo => repo.UpdateUserProfileAsync(It.Is<User>(u => u.Id == userId && u.Role == newRole)), Times.Once);
    }


}