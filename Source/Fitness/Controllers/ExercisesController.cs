using Fitness.DataAccess;
using Fitness.DataAccess.Models;
using Fitness.Models.ViewModels.Exercises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fitness.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly FitnessDbContext _context;

        public ExercisesController(FitnessDbContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            var fitnessDbContext = _context.Exercises.Include(e => e.Creator).Include(e => e.Image);
            return View(await fitnessDbContext.ToListAsync());
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.Creator)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            var viewModel = new Create
            {
                // Lade alle verfügbaren Muskelgruppen aus der DB
                AvailableMuscleGroups = _context.MuscleGroups
                    .Select(m => new SelectListItem
                        {
                            Value = m.Id.ToString(),
                            Text = m.Name
                        })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create viewModel)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                int? uploaderId = null;
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
                {
                    uploaderId = parsedId;
                }

                // Übung public?
                bool shareExercise = false;
                if (_context.Users.First(x => x.Id == uploaderId).ShareExercisesEnabled)
                {
                    shareExercise = true;
                }

                var exercise = new Exercise
                {
                    CreatorId = uploaderId,
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    VideoUrl = viewModel.YouTubeLink,
                    IsPublic = shareExercise
                };

                // 1. Bild verarbeiten und in der separaten Bild-Tabelle speichern
                if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await viewModel.ImageFile.CopyToAsync(memoryStream);

                        // 3. Datenbank-Model aus den Datei-Informationen und dem byte[] befüllen
                        var image = new Image
                        {
                            FileData = memoryStream.ToArray(), // Das eigentliche Bild
                            FileSizeBytes = viewModel.ImageFile.Length,
                            MimeType = viewModel.ImageFile.ContentType, // z.B. "image/jpeg" oder "image/png"
                            FileExtension = Path.GetExtension(viewModel.ImageFile.FileName).ToLower(),
                            UploaderId = uploaderId,
                            CreatedAt = DateTime.UtcNow
                        };

                        // 4. In der Datenbank speichern
                        _context.Images.Add(image);
                        await _context.SaveChangesAsync();

                        exercise.ImageId = image.Id; // Referenz in der Exercise setzen
                    }
                }

                // 2. Übung speichern
                _context.Exercises.Add(exercise);
                await _context.SaveChangesAsync(); // Speichern, um die Exercise.Id zu generieren

                // 3. Muskelgruppen-Zuordnungen speichern (Many-To-Many)
                if (viewModel.SelectedMuscleGroupIds != null && viewModel.SelectedMuscleGroupIds.Any())
                {
                    foreach (var muscleGroupId in viewModel.SelectedMuscleGroupIds)
                    {
                        _context.ExerciseMuscleGroups.Add(new ExerciseMuscleGroup
                        {
                            ExerciseId = exercise.Id,
                            MuscleGroupId = muscleGroupId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Falls die Validierung fehlschlägt: Dropdown-Optionen neu laden
            viewModel.AvailableMuscleGroups = _context.MuscleGroups
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList();

            return View(viewModel);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "DisplayName", exercise.CreatorId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", exercise.ImageId);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatorId,Name,Description,ImageId,VideoUrl,IsPublic,CreatedAt")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "DisplayName", exercise.CreatorId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", exercise.ImageId);
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.Creator)
                .Include(e => e.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
