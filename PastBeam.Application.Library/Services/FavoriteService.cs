﻿using PastBeam.Core.Library.Entities;
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

        public async Task AddFavoriteAsync(int userId, int articleId)
        {
            if (!await _favoriteRepository.IsArticleFavoriteAsync(userId, articleId))
            {
                await _favoriteRepository.AddFavoriteAsync(userId, articleId);
            }
        }

        public async Task RemoveFavoriteAsync(int userId, int articleId)
        {
            if (await _favoriteRepository.IsArticleFavoriteAsync(userId, articleId))
            {
                await _favoriteRepository.RemoveFavoriteAsync(userId, articleId);
            }
        }

        public async Task<bool> IsArticleFavoriteAsync(int userId, int articleId)
        {
            return await _favoriteRepository.IsArticleFavoriteAsync(userId, articleId);
        }

        public async Task<List<Article>> GetFavoritesByUserAsync(int userId)
        {
            return await _favoriteRepository.GetFavoritesByUserAsync(userId);
        }
    }
}
