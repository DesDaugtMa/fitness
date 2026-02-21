using Fitness.DataAccess;
using Fitness.DataAccess.Models;
using Fitness.Models.ViewModels.Image;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    public class ImagesController : Controller
    {
        private readonly FitnessDbContext _context; 
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImagesController(FitnessDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var fitnessDbContext = _context.Images.Include(i => i.Uploader);
            return View(await fitnessDbContext.ToListAsync());
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.Uploader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            return View(new Create());
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create model)
        {
            if (ModelState.IsValid)
            {
                if (model.File != null && model.File.Length > 0)
                {
                    // 1. Bild über einen MemoryStream in ein byte[] (Byte-Array) umwandeln
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.File.CopyToAsync(memoryStream);

                        // 2. Eingeloggten Benutzer (UploaderId) ermitteln (falls zutreffend)
                        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                        int? uploaderId = null;
                        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
                        {
                            uploaderId = parsedId;
                        }

                        // 3. Datenbank-Model aus den Datei-Informationen und dem byte[] befüllen
                        var image = new Image
                        {
                            FileData = memoryStream.ToArray(), // Das eigentliche Bild
                            FileSizeBytes = model.File.Length,
                            MimeType = model.File.ContentType, // z.B. "image/jpeg" oder "image/png"
                            FileExtension = Path.GetExtension(model.File.FileName).ToLower(),
                            Description = model.Description,
                            UploaderId = uploaderId,
                            CreatedAt = DateTime.UtcNow
                        };

                        // 4. In der Datenbank speichern
                        _context.Images.Add(image);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index", "Home"); // Nach Erfolg weiterleiten
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Bitte wähle eine gültige Datei aus.");
                }
            }

            return View(model);
        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            ViewData["UploaderId"] = new SelectList(_context.Users, "Id", "DisplayName", image.UploaderId);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UploaderId,Description,FileData,FileSizeBytes,FileExtension,MimeType,CreatedAt")] Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.Id))
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
            ViewData["UploaderId"] = new SelectList(_context.Users, "Id", "DisplayName", image.UploaderId);
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.Uploader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                _context.Images.Remove(image);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null || image.FileData == null)
            {
                return NotFound();
            }

            // Gibt die reinen Byte-Daten der Datenbank als "echte" Bild-Datei an den Browser zurück
            return File(image.FileData, image.MimeType ?? "image/jpeg");
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
