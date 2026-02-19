using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class WorkoutLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SessionId { get; set; }
        public WorkoutSession Session { get; set; } = null!;

        [Required]
        public int PlanExerciseId { get; set; }
        public PlanExercise PlanExercise { get; set; } = null!;

        [Required]
        public int SetNumber { get; set; }

        public float? ActualWeight { get; set; }

        public int? ActualReps { get; set; }

        [Required]
        public bool WasSuccessful { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
