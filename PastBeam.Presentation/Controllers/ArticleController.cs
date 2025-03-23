using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Services;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Presentation.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleService _articleService;

        public ArticleController(ArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task Index()
        {
            var articles = await _articleService.GetAllArticlesAsync();
        }

        public async Task Details(int id)
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

        public async Task UpdateArticle(int id, string? title = null, string? content = null, [FromBody] List<string>? tags = null)
        {
            var updatedArticle = await _articleService.UpdateArticleAsync(id, title, content, tags);


        }
    }
}