using System.ComponentModel.DataAnnotations;

namespace PastBeam.Application.Library.Dtos
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Введіть ім'я")] // Повідомлення про помилку можна налаштувати
        [StringLength(50, ErrorMessage = "Ім'я не може бути довшим за 50 символів")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть прізвище")]
        [StringLength(50, ErrorMessage = "Прізвище не може бути довшим за 50 символів")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть ім'я користувача")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ім'я користувача повинно містити від 3 до 100 символів")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть адресу електронної пошти")]
        [EmailAddress(ErrorMessage = "Некоректний формат електронної пошти")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити щонайменше 6 символів")]
        public string Password { get; set; } = string.Empty;

        // [Required] // Тепер не обов'язково, бо є Compare
        [DataType(DataType.Password)] // Допомагає для UI, але основна перевірка - Compare
        [Compare("Password", ErrorMessage = "Пароль та підтвердження пароля не співпадають.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true", ErrorMessage = "Ви повинні прийняти умови та положення")]
        public bool AcceptTerms { get; set; }
    }
}