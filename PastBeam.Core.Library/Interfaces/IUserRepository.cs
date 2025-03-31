using PastBeam.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Folder>> GetUserFoldersAsync(int userId);

        Task<Folder?> DeleteFolderAsync(int folderId);

        Task CreateFolderAsync(Folder folder);

        Folder? GetFolderById(int folderId);
        Task<User?> GetUserByIdAsync(int userId);

        Task UpdateUserProfileAsync(User user);
    }
}
