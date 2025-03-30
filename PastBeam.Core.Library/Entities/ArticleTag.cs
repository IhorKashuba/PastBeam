using System.ComponentModel.DataAnnotations.Schema;

namespace PastBeam.Core.Library.Entities;

[Table("article_tags")]
public class ArticleTag
{
    [ForeignKey("Article")]
    [Column("article_id")]
    public int ArticleId { get; set; }

    [ForeignKey("Tag")]
    [Column("tag_id")]
    public int TagId { get; set; }

    public Article Article { get; set; } = null!;

    public Tag Tag { get; set; } = null!;
}
