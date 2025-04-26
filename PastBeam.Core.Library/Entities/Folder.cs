using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PastBeam.Core.Library.Entities;

[Table("folders")]
public class Folder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("User")]
    [Column("user_id")]
    public string UserId { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public ICollection<FolderArticle> FolderArticles { get; set; } = new List<FolderArticle>();
}

