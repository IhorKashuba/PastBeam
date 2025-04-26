using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IUserCourseRepository
    {
        Task DeleteUserCoursesByUserAsync(string userId);
        // Додай інші методи, якщо плануєш, наприклад, додавання курсів користувачеві
    }
}