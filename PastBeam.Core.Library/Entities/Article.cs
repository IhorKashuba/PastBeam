using System.ComponentModel.DataAnnotations;

namespace PastBeam.Core.Library.Entities;

public class Article
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<string> Tags { get; set; }

    public ICollection<FavoriteArticle> FavoriteArticles { get; set; } = new List<FavoriteArticle>();

}