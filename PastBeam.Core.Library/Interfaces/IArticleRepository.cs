using PastBeam.Core.Library.Entities;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();

        Task<Article> GetByIdAsync(int id);

        Task CreateArticleAsync(Article article);

        Task UpdateArticleAsync(Article article);
    }
}
