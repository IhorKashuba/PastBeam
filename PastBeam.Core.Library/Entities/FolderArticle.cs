using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class FolderArticle
{
    [ForeignKey("Folder")]
    public int FolderId { get; set; }

    [ForeignKey("Article")]
    public int ArticleId { get; set; }

    public Folder Folder { get; set; } = null!;
    public Article Article { get; set; } = null!;
}

