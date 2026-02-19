using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class Exercise
    {
        [Key]
        public int Id { get; set; }

        public int? CreatorId { get; set; }
        public User? Creator { get; set; }

        [Required]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public int? ImageId { get; set; }
        public Image? Image { get; set; }

        public string? VideoUrl { get; set; }

        [Required]
        public bool IsPublic { get; set; } = false;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; } = new List<ExerciseMuscleGroup>();
    }
}
