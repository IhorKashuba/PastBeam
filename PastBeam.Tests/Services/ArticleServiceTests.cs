using Moq;
using PastBeam.Application.Services;
using PastBeam.Core.Entities;
using PastBeam.Core.Interfaces;
using Xunit;

public class ArticleServiceTests
{
    private readonly Mock<IArticleRepository> _mockRepo;
    private readonly ArticleService _service;

    public ArticleServiceTests()
    {
        _mockRepo = new Mock<IArticleRepository>();
        _service = new ArticleService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAllArticlesAsync_ReturnsArticles()
    {
        var articles = new List<Article> { new Article { Id = 1, Title = "Test" } };
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(articles);

        var result = await _service.GetAllArticlesAsync();

        Assert.Single(result);
    }
}
