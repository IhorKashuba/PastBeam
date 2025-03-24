using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.Library.Logger;
using PastBeam.Application.Library.Interfaces;

namespace PastBeam.Application.Library.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository repository;

        private readonly ILogger _logger;

        public ArticleService(IArticleRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await this.repository.GetAllAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await this.repository.GetByIdAsync(id);
        }

        public async Task<Article?> CreateArticle(Article article)
        {
            try
            {
                this.repository.CreateArticleAsync(article);
                return article;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error while creating article {ex}");
                throw;
            }
        }

        public async Task<Article?> UpdateArticleAsync(int id, string? title = null, string? content = null, List<string>? tags = null)
        {
            var article = await repository.GetByIdAsync(id);

            if (article == null)
                return null;

            if (title != null)
            {
                article.Title = title;
            }
            if (content != null)
            {
                article.Content = content;
            }
            if (tags != null)
            {
                article.Tags = tags;
            }

            article.UpdatedAt = DateTime.UtcNow;

            try
            {
                await repository.UpdateArticleAsync(article);
                return article;
            }
            catch (Exception ex)
            {
                _logger.LogToFile($"Error while updating article {ex}");
                throw;
            }
        }
    }
}