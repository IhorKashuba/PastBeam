using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class ArticleTag
{
    [ForeignKey("Article")]
    public int ArticleId { get; set; }

    [ForeignKey("Tag")]
    public int TagId { get; set; }

    //needed to include when searching via entity core
    public Article Article { get; set; } = null!;

    public Tag Tag { get; set; } = null!;
}
