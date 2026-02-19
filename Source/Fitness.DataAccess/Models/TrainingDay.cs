using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class TrainingDay
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PlanId { get; set; }
        public TrainingPlan Plan { get; set; } = null!;

        public string? DayName { get; set; }

        [Required]
        public int SortOrder { get; set; }

        public ICollection<PlanExercise> PlanExercises { get; set; } = new List<PlanExercise>();
    }
}
