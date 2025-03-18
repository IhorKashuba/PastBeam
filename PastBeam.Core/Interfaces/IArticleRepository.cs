using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace PastBeam.Core.Interfaces
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();

        Task<Article> GetByIdAsync(int id);

        Task CreateArticleAsync(Article article);

        Task UpdateArticleAsync(Article article);
    }
}
