using Microsoft.EntityFrameworkCore;
using PastBeam.Core.Library.Entities;

namespace PastBeam.Infrastructure.DataBase;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }

    public DbSet<Article> Articles { get; set; }

    public DbSet<Bookmark> Bookmarks { get; set; }

    public DbSet<Favorite> Favorites { get; set; }

    public DbSet<Folder> Folders { get; set; }

    public DbSet<FolderArticle> FolderArticles { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<ArticleTag> ArticleTags { get; set; }

    public DbSet<UserCourse> UserCourses { get; set; }

    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCourse>()
            .HasKey(uc => new { uc.UserId, uc.CourseId });

        modelBuilder.Entity<ArticleTag>()
            .HasKey(at => new { at.ArticleId, at.TagId });

        modelBuilder.Entity<FolderArticle>()
            .HasKey(fa => new { fa.FolderId, fa.ArticleId });

        base.OnModelCreating(modelBuilder);
    }
}
