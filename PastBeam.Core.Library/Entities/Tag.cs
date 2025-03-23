using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class Tag
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public List<ArticleTag> ArticleTags { get; set; } = new();
}
