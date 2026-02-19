using System.ComponentModel.DataAnnotations;

namespace Fitness.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Anzeigename ist erforderlich.")]
        public string DisplayName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-Mail ist erforderlich.")]
        [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Das Passwort muss mindestens 6 Zeichen lang sein.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwortbestätigung ist erforderlich.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string RegistrationToken { get; set; }
    }
}
