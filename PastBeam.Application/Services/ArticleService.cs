using PastBeam.Core.Entities;
using PastBeam.Core.Interfaces;

namespace PastBeam.Application.Services
{
    public class ArticleService
    {
        private readonly IArticleRepository repository;

        public ArticleService(IArticleRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync()
        {
            return await this.repository.GetAllAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await this.repository.GetByIdAsync(id);
        }
    }
}
//﻿namespace Pastbeam.Application.Services;

//using Core.Entities;

//public interface IArticleService 
//{
//    Task<Article> CreatePublicationAsync();

//    Task<Article> GetArticleByIdAsync();
//}

//public class ArticleService
//{
//    private readonly IArticleRepository _articleRepository;
//    private readonly ILogger<ArticleService> _logger;

//    public ArticleService(IArticleRepository articleRepository, ILogger<ArticleService> logger)
//    {
//        _articleRepository = articleRepository;
//        _logger = logger;
//    }

//    public async Task<Article> CreatePublicationAsync(CreateArticleRequest request)
//    {
//        var article = new Article
//        {
//            Id = request.Id,
//            Title = request.Title,
//            Content = request.Content,
//            CreatedAt = DateTime.UtcNow,
//            UpdatedAt = DateTime.UtcNow
//        };

//        try
//        {
//            await _articleRepository.AddAsync(article);
//            return article;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error while creating publication");
//            throw;
//        }
//    }

//    public async Task<Article> GetArticleByIdAsync(int id)
//    {
//        return await _articleRepository.GetByIdAsync(id);
//    }
//}