namespace Presentation.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using PastBeam.Application.Services;

    [ApiController]
    [Route("/article/[controller]")]
    public class ArticleController
    {
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] CreateArticleRequest request)
        {
            var article = await _articleService.CreateArticleAsync(request);
            return CreatedAtAction(nameof(GetArticleById), new { id = article.Id }, article);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticleById(int id)
        {
            var publication = await _articleService.GetArticleByIdAsync(id);
            if (publication == null)
                return NotFound();

            return Ok(publication);
        }
    }
}
