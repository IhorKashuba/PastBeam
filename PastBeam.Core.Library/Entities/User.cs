
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PastBeam.Core.Library.Entities;

[Table("user")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [Column("status")]
    public string Role { get; set; } = "User"; // Guest, User, Admin

    [Column("is_suspended")]
    public bool IsSuspended;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
    public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    public ICollection<Folder> Folders { get; set; } = new List<Folder>();
    public ICollection<FavoriteArticle> FavoriteArticles { get; set; } = new List<FavoriteArticle>();

}
