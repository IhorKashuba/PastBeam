using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.DataBase;
using System;
using System.Threading.Tasks;

namespace PastBeam.Infrastructure.Library.Repositories
{
    public class BookmarkRepository : IBookmarkRepository
    {
        private readonly ApplicationDbContext _context;

        public BookmarkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteBookmarksByUserAsync(int userId)
        {
            var bookmarks = await _context.Bookmarks
                .Where(b => b.UserId == userId)
                .ToListAsync();

            _context.Bookmarks.RemoveRange(bookmarks);
            await _context.SaveChangesAsync();
        }
    }
}