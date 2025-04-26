using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.DataBase;
using System;
using System.Threading.Tasks;

namespace PastBeam.Infrastructure.Library.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly ApplicationDbContext _context;

        public FolderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteFoldersByUserAsync(string userId)
        {
            var folders = await _context.Folders
                .Where(f => f.UserId == userId)
                .ToListAsync();

            _context.Folders.RemoveRange(folders);
            await _context.SaveChangesAsync();
        }
    }
}