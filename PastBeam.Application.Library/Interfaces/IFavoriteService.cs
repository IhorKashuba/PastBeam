using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(string userId, int articleId);
        Task RemoveFavoriteAsync(string userId, int articleId);
        Task<bool> IsArticleFavoriteAsync(string userId, int articleId);
        Task<List<Article>> GetFavoritesByUserAsync(string userId);
    }
}
