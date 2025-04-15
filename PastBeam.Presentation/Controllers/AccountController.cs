using Microsoft.AspNetCore.Mvc;
using PastBeam.Presentation.Models;

namespace PastBeam.Presentation.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // TODO: логіка реєстрації пізніше

            return RedirectToAction("Index", "Home");
        }
    }
}
