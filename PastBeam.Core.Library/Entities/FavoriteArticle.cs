using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace PastBeam.Core.Library.Entities;

[Microsoft.EntityFrameworkCore.Index(nameof(UserId), nameof(ArticleId), IsUnique = true)] // 👈 Додає унікальне обмеження
public class FavoriteArticle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("article_id")]
    public int ArticleId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [ForeignKey("ArticleId")]
    public Article Article { get; set; } = null!;
}
