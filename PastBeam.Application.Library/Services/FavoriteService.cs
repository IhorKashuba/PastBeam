using PastBeam.Core.Library.Entities;
using PastBeam.Core.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PastBeam.Application.Library.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        // Додати статтю в уподобані
        public async Task AddFavoriteAsync(int userId, int articleId)
        {
            if (!await _favoriteRepository.IsArticleFavoriteAsync(userId, articleId))
            {
                await _favoriteRepository.AddFavoriteAsync(userId, articleId);
            }
        }

        // Видалити статтю з уподобаних
        public async Task RemoveFavoriteAsync(int userId, int articleId)
        {
            if (await _favoriteRepository.IsArticleFavoriteAsync(userId, articleId))
            {
                await _favoriteRepository.RemoveFavoriteAsync(userId, articleId);
            }
        }

        // Перевірити, чи стаття вже в уподобаних
        public async Task<bool> IsArticleFavoriteAsync(int userId, int articleId)
        {
            return await _favoriteRepository.IsArticleFavoriteAsync(userId, articleId);
        }

        // Отримати всі уподобані статті користувача
        public async Task<List<Article>> GetFavoritesByUserAsync(int userId)
        {
            return await _favoriteRepository.GetFavoritesByUserAsync(userId);
        }
    }
}
