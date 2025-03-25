using PastBeam.Core.Library.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteAsync(int userId, int articleId);
        Task RemoveFavoriteAsync(int userId, int articleId);
        Task<List<Article>> GetFavoritesByUserAsync(int userId);
        Task<bool> IsArticleFavoriteAsync(int userId, int articleId);
    }
}
