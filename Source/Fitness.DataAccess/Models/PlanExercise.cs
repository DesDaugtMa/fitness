using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class PlanExercise
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TrainingDayId { get; set; }
        public TrainingDay TrainingDay { get; set; } = null!;

        [Required]
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;

        [Required]
        public int SetsCount { get; set; } = 3;

        public float? TargetMinWeight { get; set; }
        public float? TargetMaxWeight { get; set; }
        public int? TargetMinReps { get; set; }
        public int? TargetMaxReps { get; set; }

        public float PoWeightIncrement { get; set; } = 0;
        public int PoRepsIncrement { get; set; } = 0;
        public bool PoActive { get; set; } = false;

        public string? Note { get; set; }

        [Required]
        public int SortOrder { get; set; }
    }
}
