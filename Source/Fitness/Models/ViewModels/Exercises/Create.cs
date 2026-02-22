using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Models.ViewModels.Exercises
{
    public class Create
    {
        [Required(ErrorMessage = "Ein Name ist erforderlich.")]
        [Display(Name = "Übungsname")]
        public string Name { get; set; }

        [Display(Name = "Beschreibung")]
        public string? Description { get; set; }

        [Display(Name = "Bild hochladen")]
        public IFormFile? ImageFile { get; set; } // Die eigentliche hochgeladene Datei

        [Display(Name = "YouTube-Link")]
        [Url(ErrorMessage = "Bitte gib eine gültige URL ein.")]
        public string? YouTubeLink { get; set; }

        // Speichert die IDs der ausgewählten Muskelgruppen aus dem Formular
        [Display(Name = "Muskelgruppen")]
        public List<int> SelectedMuscleGroupIds { get; set; } = new List<int>();

        // Stellt die Optionen für das Dropdown-Menü in der View bereit
        public List<SelectListItem> AvailableMuscleGroups { get; set; } = new List<SelectListItem>();
    }
}
