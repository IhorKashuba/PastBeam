using PastBeam.Core.Library.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IFolderRepository
    {
        Task DeleteFoldersByUserAsync(int userId);
        // Додай інші методи, якщо потрібно: CreateFolder, GetFoldersByUser, тощо
    }
}