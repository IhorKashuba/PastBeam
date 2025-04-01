using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PastBeam.Infrastructure.DataBase;
using PastBeam.Core.Library.Interfaces;



namespace PastBeam.Infrastructure.Library.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsArticleFavoriteAsync(int userId, int articleId)
        {
            return await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.ArticleId == articleId);
        }

        public async Task AddFavoriteAsync(int userId, int articleId)
        {
            var favorite = new Favorite
            {
                UserId = userId,
                ArticleId = articleId
            };

            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(int userId, int articleId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ArticleId == articleId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Article>> GetFavoritesByUserAsync(int userId)
        {
            return await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Article)
                .ToListAsync();
        }
    }
}

