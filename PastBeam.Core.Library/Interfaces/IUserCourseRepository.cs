using System.Threading.Tasks;

namespace PastBeam.Core.Library.Interfaces
{
    public interface IUserCourseRepository
    {
        Task DeleteUserCoursesByUserAsync(int userId);
        // Додай інші методи, якщо плануєш, наприклад, додавання курсів користувачеві
    }
}