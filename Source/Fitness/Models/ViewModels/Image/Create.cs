using System.ComponentModel.DataAnnotations;

namespace Fitness.Models.ViewModels.Image
{
    public class Create
    {
        [Required(ErrorMessage = "Bitte wähle ein Bild aus.")]
        [Display(Name = "Bild")]
        public IFormFile? File { get; set; }

        [Display(Name = "Beschreibung")]
        [MaxLength(500, ErrorMessage = "Die Beschreibung darf maximal 500 Zeichen lang sein.")]
        public string? Description { get; set; }
    }
}
