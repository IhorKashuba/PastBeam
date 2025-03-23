using Core.Entities;
using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Interfaces;
using PastBeam.Infrastructure.DataBase;

namespace PastBeam.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task CreateArticleAsync(Article newArticle)
        {
            _context.Articles.Add(newArticle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync( Article updatedArticle)
        {
            _context.Articles.Update(updatedArticle);
            await _context.SaveChangesAsync();
        }
    }
}