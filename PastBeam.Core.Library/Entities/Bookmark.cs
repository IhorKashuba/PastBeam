using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PastBeam.Core.Library.Entities;

[Table("bookmarks")]
public class Bookmark
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("User")]
    [Column("user_id")]
    public string UserId { get; set; }

    [ForeignKey("Article")]
    [Column("article_id")]
    public int ArticleId { get; set; }

    //needed to include when searching via entity core
    public User User { get; set; } = null!;
    public Article Article { get; set; } = null!;
}
