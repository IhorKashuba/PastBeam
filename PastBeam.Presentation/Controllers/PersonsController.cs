using Microsoft.AspNetCore.Mvc;

// Якщо твій проект має інший простір імен для контролерів, зміни його.
// Наприклад, якщо ти використовував PastBeam.Presentation.Controllers для UserController
namespace PastBeam.Presentation.Controllers
{
    // Атрибут [Route] визначає базовий маршрут для всіх дій в цьому контролері.
    // Наприклад, доступ до дії Index буде за URL /persons або /persons/index
    [Route("persons")]
    public class PersonsController : Controller
    {
        // На майбутнє, тут будуть залежності, наприклад, сервіс для роботи з даними про особистостей:
        // private readonly IPersonService _personService;
        // public PersonsController(IPersonService personService)
        // {
        // _personService = personService;
        // }

        // Дія, яка буде відображати список особистостей.
        // HTTP GET запит на /persons або /persons/index потрапить сюди.
        [HttpGet] // Можна вказати [HttpGet("")] або [HttpGet("index")] якщо потрібно уточнити
        public IActionResult Index()
        {
            // На цьому етапі ми просто повертаємо представлення.
            // У майбутньому тут буде логіка для отримання списку особистостей
            // з _personService та передача їх у представлення як модель.
            //
            // Приклад, як це може виглядати пізніше:
            // var personsList = await _personService.GetAllPersonsAsync();
            // return View(personsList); 

            // Поки що, оскільки ми робимо візуал зі статичними даними у View, 
            // модель передавати не обов'язково.
            return View();
        }

        // На майбутнє, тут може бути дія для пошуку, на яку посилається форма пошуку
        // [HttpGet("search")]
        // public async Task<IActionResult> Search(string query)
        // {
        //     // Логіка пошуку
        //     // var searchResults = await _personService.SearchPersonsAsync(query);
        //     // return View("Index", searchResults); // Можна повернути те саме представлення Index, але з результатами пошуку
        //     return View("Index"); // Поки що заглушка
        // }

        // Тут також можуть бути дії для перегляду деталей конкретної особистості,
        // додавання/редагування (якщо це буде робити адмін) тощо.
    }
}