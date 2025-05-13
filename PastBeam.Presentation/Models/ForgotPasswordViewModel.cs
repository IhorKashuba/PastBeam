using System.ComponentModel.DataAnnotations;

namespace PastBeam.Presentation.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}