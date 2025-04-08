using PastBeam.Core.Library.Entities;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IBookmarkRepository
    {
        Task DeleteBookmarksByUserAsync(int userId);
        // Додай інші методи, якщо потрібно: GetByUser, AddBookmark, тощо
    }
}