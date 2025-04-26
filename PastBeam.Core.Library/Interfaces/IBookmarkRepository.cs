using PastBeam.Core.Library.Entities;
using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IBookmarkRepository
    {
        Task DeleteBookmarksByUserAsync(string userId);
    }
}