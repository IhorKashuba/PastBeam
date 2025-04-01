using Moq;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Logger;
using System;
using System.Threading.Tasks;
using Xunit;

public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _mockCourseRepository;
    private readonly Mock<ILogger> _mockLogger;
    private readonly CourseService _courseService;

    public CourseServiceTests()
    {
        _mockCourseRepository = new Mock<ICourseRepository>();
        _mockLogger = new Mock<ILogger>();
        _courseService = new CourseService(_mockCourseRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCourseProgressAsync_UserEnrolled_ReturnsProgress()
    {
        // Arrange
        var userId = 1;
        var courseId = 10;
        var expectedProgress = 0.75f; // 75%
        var userCourseDetails = new UserCourse { UserId = userId, CourseId = courseId, Progress = expectedProgress };

        _mockCourseRepository.Setup(repo => repo.GetUserCourseDetailsAsync(userId, courseId))
                             .ReturnsAsync(userCourseDetails);

        // Act
        var result = await _courseService.GetCourseProgressAsync(userId, courseId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProgress, result.Value);
        _mockCourseRepository.Verify(repo => repo.GetUserCourseDetailsAsync(userId, courseId), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Attempting to get progress for user {userId} on course {courseId}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Progress for user {userId} on course {courseId} is {expectedProgress}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("not enrolled"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error getting progress"))), Times.Never);
    }

    [Fact]
    public async Task GetCourseProgressAsync_UserNotEnrolled_ReturnsNull()
    {
        // Arrange
        var userId = 1;
        var courseId = 10;

        _mockCourseRepository.Setup(repo => repo.GetUserCourseDetailsAsync(userId, courseId))
                             .ReturnsAsync((UserCourse?)null); // Simulate user not enrolled

        // Act
        var result = await _courseService.GetCourseProgressAsync(userId, courseId);

        // Assert
        Assert.Null(result);
        _mockCourseRepository.Verify(repo => repo.GetUserCourseDetailsAsync(userId, courseId), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Attempting to get progress for user {userId} on course {courseId}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"User {userId} is not enrolled in course {courseId}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Progress for user"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("Error getting progress"))), Times.Never);
    }

    [Fact]
    public async Task GetCourseProgressAsync_RepositoryThrowsException_RethrowsAndLogsError()
    {
        // Arrange
        var userId = 1;
        var courseId = 10;
        var repositoryException = new InvalidOperationException("Database connection failed");

        _mockCourseRepository.Setup(repo => repo.GetUserCourseDetailsAsync(userId, courseId))
                             .ThrowsAsync(repositoryException);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() => _courseService.GetCourseProgressAsync(userId, courseId));

        Assert.Same(repositoryException, actualException); // Verify original exception is rethrown
        _mockCourseRepository.Verify(repo => repo.GetUserCourseDetailsAsync(userId, courseId), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Attempting to get progress for user {userId} on course {courseId}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Error getting progress for user {userId} on course {courseId}"))), Times.Once);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains("not enrolled"))), Times.Never);
        _mockLogger.Verify(log => log.LogToFile(It.Is<string>(s => s.Contains($"Progress for user"))), Times.Never);
    }
}