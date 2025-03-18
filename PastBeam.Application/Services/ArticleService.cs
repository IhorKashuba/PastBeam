using Core.Entities;
using PastBeam.Core.Interfaces;
using ILogger = PastBeam.Infrastructure.Logger.ILogger;

namespace PastBeam.Application.Services
{
    public class ArticleService
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

        public async Task<Article?> UpdateArticle(int id, Article updatedArticle)
        {
            var article = await repository.GetByIdAsync(id);

            if (article == null)
                return null;

            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
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