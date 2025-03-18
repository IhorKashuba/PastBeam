using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PastBeam.Application.Services;
using PastBeam.Core.Entities;
using System.Threading.Tasks;

namespace PastBeam.Presentation.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ArticleService articleService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all articles...");
            var articles = await _articleService.GetAllArticlesAsync();
            return View(articles);
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation($"Fetching article with ID: {id}");
            var article = await _articleService.GetArticleByIdAsync(id);

            if (article == null)
            {
                _logger.LogWarning($"Article with ID {id} not found.");
                return NotFound();
            }

            return View(article);
        }
    }
}