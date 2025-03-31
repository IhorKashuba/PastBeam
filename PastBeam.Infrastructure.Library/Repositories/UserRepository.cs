using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Infrastructure.DataBase;

namespace PastBeam.Infrastructure.Library.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task DeleteAsync(int userId)
        {
            var userToDelete = await _context.Users.FindAsync(userId);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Folder>> GetUserFoldersAsync(int userId)
        {
            return await _context.Folders.Where(f => f.UserId == userId).ToListAsync();
        }

        public async Task<Folder?> DeleteFolderAsync(int folderId)
        {
            Folder? existingFolder = await _context.Folders.FirstOrDefaultAsync(f => f.Id == folderId);

            if (existingFolder != null)
            {
                _context.Folders.Remove(existingFolder);
                await _context.SaveChangesAsync();
                return existingFolder;
            }
            else
            {
                throw new KeyNotFoundException("Folder not found or access denied.");
            }
        }

        public async Task CreateFolderAsync(Folder folder)
        {
            Folder? existingFolder = await _context.Folders.FirstOrDefaultAsync(f => f.Name == folder.Name && f.UserId == folder.UserId);

            if (existingFolder == null)
            {
                _context.Folders.Add(folder);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Error: It is not allowed to create folders with the same name and the same user");
            }
        }

        public Folder? GetFolderById(int folderId)
        {
            Folder? folder = _context.Folders.FirstOrDefault(f => f.Id == folderId);

            if (folder != null)
            {
                return folder;
            }
            else
            {
                throw new KeyNotFoundException("there is no folder with this id");
            }
        }
    }
}
