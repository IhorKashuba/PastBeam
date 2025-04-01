using Moq;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using Xunit;

public class ArticleServiceTests
{
    private readonly Mock<IArticleRepository> articleRepositoryMock;
    private readonly ArticleService articleService;

    public ArticleServiceTests()
    {
        articleRepositoryMock = new Mock<IArticleRepository>();
        articleService = new ArticleService(articleRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllArticlesAsync_ReturnsArticles()
    {
        // Arrange
        var articles = new List<Article> { new Article { Id = 1, Title = "Test Article" } };
        this.articleRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(articles);

        // Act
        var result = await this.articleService.GetAllArticlesAsync();
        List<Article> articleList = result.ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Article", articleList[0].Title);
    }

    [Fact]
    public async Task GetArticleByIdAsync_ReturnsNull_WhenArticleDoesNotExist()
    {
        // Arrange
        this.articleRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Article?)null);

        // Act
        var result = await this.articleService.GetArticleByIdAsync(99);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateArticleAsyncTest()
    {
        Article article = new Article
        {
            Title = "title",
            Content = "test content",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        this.articleRepositoryMock.Setup(repo => repo.CreateArticleAsync(It.IsAny<Article>())).Returns(Task.CompletedTask);

        var result = await this.articleService.CreateArticle(article);

        Assert.NotNull(result);
        Assert.Equal(article, result);
        this.articleRepositoryMock.Verify(repo => repo.CreateArticleAsync(It.IsAny<Article>()), Times.Once);
    }

    [Fact]
    public async Task UpdateArticleAsync_ShouldReturnUpdatedArticle_WhenArticleExists()
    {
        // Arrange
        var existingArticle = new Article { Id = 1, Title = "Old Title", Content = "Old Content", UpdatedAt = DateTime.UtcNow.AddDays(-1) };
        var updatedTitle = "Updated Title";
        var updatedContent = "Updated Content";

        articleRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingArticle);
        articleRepositoryMock.Setup(repo => repo.UpdateArticleAsync(It.IsAny<Article>())).Returns(Task.CompletedTask);

        // Act
        var result = await articleService.UpdateArticleAsync(1, updatedTitle, updatedContent);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedTitle, result?.Title);
        Assert.Equal(updatedContent, result?.Content);
        Assert.True(result?.UpdatedAt >= existingArticle.UpdatedAt); //create and assert at the same time
        articleRepositoryMock.Verify(repo => repo.UpdateArticleAsync(It.Is<Article>(a => a.Title == updatedTitle && a.Content == updatedContent)), Times.Once);
    }

    [Fact]
    public async Task UpdateArticleAsync_ShouldReturnNull_WhenArticleNotFound()
    {
        // Arrange
        articleRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Article?)null);

        // Act
        var result = await articleService.UpdateArticleAsync(1, "Updated Title", "Updated Content");

        // Assert
        Assert.Null(result);
        articleRepositoryMock.Verify(repo => repo.UpdateArticleAsync(It.IsAny<Article>()), Times.Never);
    }

}
