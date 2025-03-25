using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(int userId, int articleId);
        Task RemoveFavoriteAsync(int userId, int articleId);
        Task<bool> IsArticleFavoriteAsync(int userId, int articleId);
        Task<List<Article>> GetFavoritesByUserAsync(int userId);
    }
}
