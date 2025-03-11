using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Folder
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public List<FolderArticle> FolderArticles { get; set; } = new();
}

