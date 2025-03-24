using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Presentation.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task GetAllArticles()
        {
            var articles = await _articleService.GetAllArticlesAsync();
        }

        public async Task GetArticleDetails(int id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);
        }

        public async Task CreateArticle(string title, string content, List<string> tags)
        {
            Article article = new Article
            {
                Title = title,
                Content = content,
                Tags = tags,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _articleService.CreateArticle(article);
        }

        public async Task UpdateArticle(int id, [FromBody] string? title = null, [FromBody] string? content = null, [FromBody] List<string>? tags = null)
        {
            await _articleService.UpdateArticleAsync(id, title, content, tags);
        }
    }
}