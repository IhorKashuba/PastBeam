using PastBeam.Core.Library.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IFolderRepository
    {
        Task DeleteFoldersByUserAsync(string userId);

        Task<List<Article>?> GetFolderArticleAsync(int folderId);

        Task<Folder?> GetFolderAsync(int folderId);
    }
}