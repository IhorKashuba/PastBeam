using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Bookmark
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Article")]
    public int ArticleId { get; set; }

    //needed to include when searching via entity core
    public User User { get; set; } = null!;
    public Article Article { get; set; } = null!;
}
