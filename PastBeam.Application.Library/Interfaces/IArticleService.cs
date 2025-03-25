using PastBeam.Core.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAllArticlesAsync();

        Task<Article?> GetArticleByIdAsync(int id);

        Task<Article?> CreateArticle(Article article);

        Task<Article?> UpdateArticleAsync(int id, string? title = null, string? content = null, List<string>? tags = null);
    }
}
