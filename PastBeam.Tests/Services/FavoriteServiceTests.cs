using Moq;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Interfaces;
using Xunit;
using System.Threading.Tasks;

public class FavoriteServiceTests
{
    private readonly Mock<IFavoriteRepository> _mockRepo;
    private readonly FavoriteService _service;

    public FavoriteServiceTests()
    {
        _mockRepo = new Mock<IFavoriteRepository>();
        _service = new FavoriteService(_mockRepo.Object);
    }

    [Fact]
    public async Task AddFavoriteAsync_ShouldCallAddFavorite_WhenArticleIsNotFavorite()
    {
        // Arrange
        string userId = "testid";
        int articleId = 100;
        _mockRepo.Setup(r => r.IsArticleFavoriteAsync(userId, articleId)).ReturnsAsync(false);

        // Act
        await _service.AddFavoriteAsync(userId, articleId);

        // Assert
        _mockRepo.Verify(r => r.AddFavoriteAsync(userId, articleId), Times.Once);
    }

    [Fact]
    public async Task AddFavoriteAsync_ShouldNotCallAddFavorite_WhenArticleIsAlreadyFavorite()
    {
        // Arrange
        string userId = "testid";
        int articleId = 100;
        _mockRepo.Setup(r => r.IsArticleFavoriteAsync(userId, articleId)).ReturnsAsync(true);

        // Act
        await _service.AddFavoriteAsync(userId, articleId);

        // Assert
        _mockRepo.Verify(r => r.AddFavoriteAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
    }
}
