using Core.Entities;
using PastBeam.Core.Interfaces;

namespace PastBeam.Application.Services
{
    public class ArticleService
    {
        private readonly IArticleRepository repository;

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
    }
}