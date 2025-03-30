using System.ComponentModel.DataAnnotations.Schema;

namespace PastBeam.Core.Library.Entities;

[Table("folder_articles")]
public class FolderArticle
{
    [ForeignKey("Folder")]
    [Column("folder_id")]
    public int FolderId { get; set; }

    [ForeignKey("Article")]
    [Column("article_id")]
    public int ArticleId { get; set; }

    public Folder Folder { get; set; } = null!;
    public Article Article { get; set; } = null!;
}

