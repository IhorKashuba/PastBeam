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

        public async Task<bool> IsArticleFavoriteAsync(string userId, int articleId)
        {
            return await _context.FavoriteArticles
                .AnyAsync(f => f.UserId == userId && f.ArticleId == articleId);
        }

        public async Task AddFavoriteAsync(string userId, int articleId)
        {
            var favorite = new FavoriteArticle
            {
                UserId = userId,
                ArticleId = articleId
            };

            await _context.FavoriteArticles.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoriteAsync(string userId, int articleId)
        {
            var favorite = await _context.FavoriteArticles
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ArticleId == articleId);

            if (favorite != null)
            {
                _context.FavoriteArticles.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Article>> GetFavoritesByUserAsync(string userId)
        {
            return await _context.FavoriteArticles
                .Where(f => f.UserId == userId)
                .Select(f => f.Article)
                .ToListAsync();
        }

        public async Task DeleteFavoritesByUserAsync(string userId)
        {
            var favorites = _context.FavoriteArticles.Where(f => f.UserId == userId);
            _context.FavoriteArticles.RemoveRange(favorites);
            await _context.SaveChangesAsync();
        }
    }
}

