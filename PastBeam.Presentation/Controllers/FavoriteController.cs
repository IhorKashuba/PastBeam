using Microsoft.AspNetCore.Mvc;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using System;
using System.Threading.Tasks;

namespace PastBeam.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        // Додати статтю в уподобані
        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int userId, int articleId)
        {
            try
            {
                await _favoriteService.AddFavoriteAsync(userId, articleId);

                return RedirectToAction("Index"); // Направляє назад на головну сторінку (або іншу)
            }
            catch
            {
                // Обробка помилок
                ModelState.AddModelError("", "Не вдалося додати в уподобані.");
                return View();
            }
        }

        // Видалити статтю з уподобаних
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorites(int userId, int articleId)
        {
            try
            {
                await _favoriteService.RemoveFavoriteAsync(userId, articleId);
                return RedirectToAction("Index"); // Направляє назад на головну сторінку (або іншу)
            }
            catch
            {
                // Обробка помилок
                ModelState.AddModelError("", "Не вдалося видалити з уподобаних.");
                return View();
            }
        }

        // Отримати всі уподобані статті
        [HttpGet]
        public async Task<IActionResult> GetFavorites(int userId)
        {
            try
            {
                var favorites = await _favoriteService.GetFavoritesByUserAsync(userId);
                return View(favorites); // Повертаємо список статей в View
            }
            catch
            {
                ModelState.AddModelError("", "Не вдалося отримати уподобані статті.");
                return View();
            }
        }
    }
}
