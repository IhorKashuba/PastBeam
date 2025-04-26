using PastBeam.Core.Library.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IFavoriteRepository
    {
        Task AddFavoriteAsync(string userId, int articleId);
        Task RemoveFavoriteAsync(string userId, int articleId);
        Task<List<Article>> GetFavoritesByUserAsync(string userId);
        Task<bool> IsArticleFavoriteAsync(string userId, int articleId);
        Task DeleteFavoritesByUserAsync(string userId);
    }
}
