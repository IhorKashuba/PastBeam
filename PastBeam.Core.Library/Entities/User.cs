using System.ComponentModel.DataAnnotations;

namespace PastBeam.Core.Library.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "User"; // Guest, User, Admin

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<UserCourse> UserCourses { get; set; } = new();
    public List<Bookmark> Bookmarks { get; set; } = new();
    public List<Favorite> Favorites { get; set; } = new();
    public List<Folder> Folders { get; set; } = new();
}
