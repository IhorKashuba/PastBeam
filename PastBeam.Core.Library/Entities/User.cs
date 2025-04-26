
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PastBeam.Core.Library.Entities;

public class User : IdentityUser
{
    //[Key]
    //[Column("id")]
    //public int Id { get; set; }

    [Required]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    //[Required, EmailAddress]
    //[Column("email")]
    //public string Email { get; set; } = string.Empty;

    //[Required]
    //[Column("password")]
    //public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User"; // Guest, User, Admin

    [Column("IsSuspended")]
    public bool IsSuspended;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<Folder> Folders { get; set; } = new List<Folder>();
    public ICollection<FavoriteArticle> FavoriteArticles { get; set; } = new List<FavoriteArticle>();

}
