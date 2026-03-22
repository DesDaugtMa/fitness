using System.Collections.Generic;

namespace Fitness.Models.DTOs
{
    public class SavePlanDto
    {
        public string PlanName { get; set; } = string.Empty;
        public string? PlanDesc { get; set; }
        public List<TrainingDayDto> Days { get; set; } = new List<TrainingDayDto>();
    }

    public class TrainingDayDto
    {
        public string Name { get; set; } = string.Empty;
        public bool IsRestDay { get; set; }
        public List<PlanExerciseDto> Exercises { get; set; } = new List<PlanExerciseDto>();
    }

    public class PlanExerciseDto
    {
        public int Id { get; set; } 
        public int Sets { get; set; }
        public int Reps { get; set; }
        public bool PoActive { get; set; }
        public int? PoTargetMinReps { get; set; }
        public int? PoTargetMaxReps { get; set; }
        public float? PoWeightIncrement { get; set; }
        public int? PoRepsIncrement { get; set; }
    }
}
