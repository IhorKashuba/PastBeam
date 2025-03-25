﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace PastBeam.Core.Library.Entities;

[Index(nameof(UserId), nameof(ArticleId), IsUnique = true)] // 👈 Додає унікальне обмеження
public class FavoriteArticle
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ArticleId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [ForeignKey("ArticleId")]
    public Article Article { get; set; } = null!;
}
