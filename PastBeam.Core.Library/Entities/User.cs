using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PastBeam.Core.Library.Entities;

[Table("user")]
public class User : IdentityUser<int>
{
    [Column("status")]
    public string Role { get; set; } = "User"; // Guest, User, Admin

    [Column("is_suspended")]
    public bool IsSuspended;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Навігаційні властивості — залишаємо
    public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<Folder> Folders { get; set; } = new List<Folder>();
    public ICollection<FavoriteArticle> FavoriteArticles { get; set; } = new List<FavoriteArticle>();
}
