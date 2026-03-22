using Fitness.DataAccess;
using Fitness.DataAccess.Models;
using Fitness.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fitness.Controllers
{
    [Authorize]
    public class TrainingPlansController : Controller
    {
        private readonly FitnessDbContext _context;

        public TrainingPlansController(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create()
        {
            // Lade alle öffentlichen Übungen oder eigene Übungen
            var currentUserIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? currentUserId = null;
            if (int.TryParse(currentUserIdStr, out int parsed))
                currentUserId = parsed;

            var exercisesQuery = _context.Exercises
                .Include(e => e.ExerciseMuscleGroups)
                    .ThenInclude(emg => emg.MuscleGroup)
                .AsQueryable();
            
            // Filter: Nur öffentliche oder eigene
            if (currentUserId.HasValue)
            {
                exercisesQuery = exercisesQuery.Where(e => e.IsPublic || e.CreatorId == currentUserId.Value);
            }
            else
            {
                exercisesQuery = exercisesQuery.Where(e => e.IsPublic);
            }

            var exercises = await exercisesQuery
                .Select(e => new 
                {
                    id = e.Id,
                    name = e.Name,
                    muscle = e.ExerciseMuscleGroups.Select(m => m.MuscleGroup.Name).ToList(),
                    image = e.ImageId != null ? $"/Images/GetImage/{e.ImageId}" : "https://placehold.co/100x100/3b82f6/white?text=NoImg"
                })
                .ToListAsync();

            ViewBag.ExercisesJson = JsonSerializer.Serialize(exercises, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SavePlanDto planData)
        {
            if (planData == null || string.IsNullOrWhiteSpace(planData.PlanName))
                return BadRequest(new { success = false, message = "Der Name des Plans ist erforderlich." });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized();

            var plan = new TrainingPlan
            {
                UserId = userId,
                Title = planData.PlanName,
                Description = planData.PlanDesc,
                CreatedAt = DateTime.UtcNow
            };

            _context.TrainingPlans.Add(plan);

            int daySortOrder = 0;
            if (planData.Days != null)
            {
                foreach (var dayDto in planData.Days)
                {
                    var day = new TrainingDay
                    {
                        Plan = plan,
                        DayName = dayDto.Name,
                        IsRestDay = dayDto.IsRestDay,
                        SortOrder = daySortOrder++
                    };

                    _context.TrainingDays.Add(day);

                    if (!dayDto.IsRestDay && dayDto.Exercises != null)
                    {
                        int exSortOrder = 0;
                        foreach (var exDto in dayDto.Exercises)
                        {
                            var planEx = new PlanExercise
                            {
                                TrainingDay = day,
                                ExerciseId = exDto.Id,
                                SetsCount = exDto.Sets,
                                RepsCount = exDto.Reps,
                                PoActive = exDto.PoActive,
                                TargetMinReps = exDto.PoTargetMinReps,
                                TargetMaxReps = exDto.PoTargetMaxReps,
                                PoWeightIncrement = exDto.PoWeightIncrement ?? 0f,
                                PoRepsIncrement = exDto.PoRepsIncrement ?? 0,
                                SortOrder = exSortOrder++
                            };
                            _context.PlanExercises.Add(planEx);
                        }
                    }
                }
            }

            // Validierung
            if (planData.Days == null || !planData.Days.Any(d => !d.IsRestDay))
                return BadRequest(new { success = false, message = "Ein Plan muss mindestens einen aktiven Trainingstag haben." });

            await _context.SaveChangesAsync();

            return Ok(new { success = true, planId = plan.Id, message = "Trainingsplan erfolgreich gespeichert!" });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
                return Unauthorized();

            var plan = await _context.TrainingPlans
                .Include(p => p.TrainingDays)
                    .ThenInclude(td => td.PlanExercises)
                        .ThenInclude(pe => pe.Exercise)
                            .ThenInclude(e => e.ExerciseMuscleGroups)
                                .ThenInclude(emg => emg.MuscleGroup)
                .Include(p => p.TrainingDays)
                    .ThenInclude(td => td.PlanExercises)
                        .ThenInclude(pe => pe.Exercise)
                            .ThenInclude(e => e.Image)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == currentUserId);

            if (plan == null) return NotFound();

            return View(plan);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
                return Unauthorized();

            var plan = await _context.TrainingPlans
                .Include(p => p.TrainingDays)
                    .ThenInclude(td => td.PlanExercises)
                        .ThenInclude(pe => pe.Exercise)
                            .ThenInclude(e => e.ExerciseMuscleGroups)
                                .ThenInclude(emg => emg.MuscleGroup)
                .Include(p => p.TrainingDays)
                    .ThenInclude(td => td.PlanExercises)
                        .ThenInclude(pe => pe.Exercise)
                            .ThenInclude(e => e.Image)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == currentUserId);

            if (plan == null) return NotFound();

            var initialData = new 
            {
                planName = plan.Title,
                planDesc = plan.Description,
                days = plan.TrainingDays.OrderBy(d => d.SortOrder).Select(d => new 
                {
                    id = "id-" + Guid.NewGuid().ToString().Substring(0, 8),
                    name = d.DayName ?? "",
                    isRestDay = d.IsRestDay,
                    exercises = d.PlanExercises.OrderBy(pe => pe.SortOrder).Select(pe => new 
                    {
                        instanceId = "id-" + Guid.NewGuid().ToString().Substring(0, 8),
                        id = pe.ExerciseId,
                        name = pe.Exercise.Name,
                        image = pe.Exercise.ImageId != null ? $"/Images/GetImage/{pe.Exercise.ImageId}" : "https://placehold.co/100x100/3b82f6/white?text=NoImg",
                        muscle = pe.Exercise.ExerciseMuscleGroups.Select(m => m.MuscleGroup.Name).ToList(),
                        sets = pe.SetsCount,
                        reps = pe.RepsCount,
                        poActive = pe.PoActive,
                        poTargetMinReps = pe.TargetMinReps ?? 8,
                        poTargetMaxReps = pe.TargetMaxReps ?? 12,
                        poWeightIncrement = pe.PoWeightIncrement,
                        poRepsIncrement = pe.PoRepsIncrement
                    }).ToList()
                }).ToList()
            };

            ViewBag.InitialPlanDataJson = JsonSerializer.Serialize(initialData, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            
            // Lade Übungen für Dropdown genau wie in Create
            var exercisesQuery = _context.Exercises
                .Include(e => e.ExerciseMuscleGroups)
                    .ThenInclude(emg => emg.MuscleGroup)
                .AsQueryable();
            
            exercisesQuery = exercisesQuery.Where(e => e.IsPublic || e.CreatorId == currentUserId);

            var exercises = await exercisesQuery
                .Select(e => new 
                {
                    id = e.Id,
                    name = e.Name,
                    muscle = e.ExerciseMuscleGroups.Select(m => m.MuscleGroup.Name).ToList(),
                    image = e.ImageId != null ? $"/Images/GetImage/{e.ImageId}" : "https://placehold.co/100x100/3b82f6/white?text=NoImg"
                })
                .ToListAsync();

            ViewBag.ExercisesJson = JsonSerializer.Serialize(exercises, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            
            return View(plan.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromBody] SavePlanDto planData)
        {
            if (planData == null || string.IsNullOrWhiteSpace(planData.PlanName))
                return BadRequest(new { success = false, message = "Der Name des Plans ist erforderlich." });

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized();

            var plan = await _context.TrainingPlans
                .Include(p => p.TrainingDays)
                    .ThenInclude(d => d.PlanExercises)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plan == null) return NotFound(new { success = false, message = "Plan nicht gefunden." });

            plan.Title = planData.PlanName;
            plan.Description = planData.PlanDesc;

            _context.TrainingDays.RemoveRange(plan.TrainingDays);

            int daySortOrder = 0;
            if (planData.Days != null)
            {
                foreach (var dayDto in planData.Days)
                {
                    var day = new TrainingDay
                    {
                        PlanId = plan.Id,
                        DayName = dayDto.Name,
                        IsRestDay = dayDto.IsRestDay,
                        SortOrder = daySortOrder++
                    };
                    
                    _context.TrainingDays.Add(day);

                    if (!dayDto.IsRestDay && dayDto.Exercises != null)
                    {
                        int exSortOrder = 0;
                        foreach (var exDto in dayDto.Exercises)
                        {
                            var planEx = new PlanExercise
                            {
                                TrainingDay = day,
                                ExerciseId = exDto.Id,
                                SetsCount = exDto.Sets,
                                RepsCount = exDto.Reps,
                                PoActive = exDto.PoActive,
                                TargetMinReps = exDto.PoTargetMinReps,
                                TargetMaxReps = exDto.PoTargetMaxReps,
                                PoWeightIncrement = exDto.PoWeightIncrement ?? 0f,
                                PoRepsIncrement = exDto.PoRepsIncrement ?? 0,
                                SortOrder = exSortOrder++
                            };
                            _context.PlanExercises.Add(planEx);
                        }
                    }
                }
            }

            if (planData.Days == null || !planData.Days.Any(d => !d.IsRestDay))
                return BadRequest(new { success = false, message = "Ein Plan muss mindestens einen aktiven Trainingstag haben." });

            await _context.SaveChangesAsync();

            return Ok(new { success = true, planId = plan.Id, message = "Trainingsplan erfolgreich aktualisiert!" });
        }
    }
}
