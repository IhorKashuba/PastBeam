using Moq;
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
}