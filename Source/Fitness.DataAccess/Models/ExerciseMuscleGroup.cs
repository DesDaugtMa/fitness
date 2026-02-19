using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Fitness.DataAccess.Models
{
    public class ExerciseMuscleGroup
    {
        public int ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;

        public int MuscleGroupId { get; set; }
        public MuscleGroup MuscleGroup { get; set; } = null!;

        [Required]
        public bool IsPrimary { get; set; } = true;
    }
}
