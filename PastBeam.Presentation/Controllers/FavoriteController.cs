using Microsoft.AspNetCore.Mvc;
using PastBeam.Core.Library.Interfaces;
using PastBeam.Core.Library.Entities;
using System;
using System.Threading.Tasks;

namespace PastBeam.Controllers
{
    [Route("favorite")]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("add/{articleId}")]
        public async Task<IActionResult> AddToFavorites(string userId, int articleId)
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
        [HttpPost("remove/{articleId}")]
        public async Task<IActionResult> RemoveFromFavorites(string userId, int articleId)
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
        [HttpGet("all")]
        public async Task<IActionResult> GetFavorites(string userId)
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
