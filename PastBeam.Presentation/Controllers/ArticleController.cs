using Microsoft.AspNetCore.Mvc;
using PastBeam.Application.Library.Interfaces;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Presentation.Controllers
{
    [Route("article")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return View("ArticleList", articles);
        }

        public async Task<IActionResult> GetArticleDetails(int id)
        {
            Article? article = await _articleService.GetArticleByIdAsync(id);
            return View("Details", article);
        }

        public async Task CreateArticle(string title, string content, List<string> tags)
        {
            Article article = new Article
            {
                Title = title,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _articleService.CreateArticle(article);
        }

        public async Task UpdateArticle(int id, [FromBody] string? title = null, [FromBody] string? content = null)
        {
            await _articleService.UpdateArticleAsync(id, title, content);
        }
    }
}