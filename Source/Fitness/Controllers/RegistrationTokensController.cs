using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fitness.DataAccess;
using Fitness.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fitness.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegistrationTokensController : Controller
    {
        private readonly FitnessDbContext _context;

        public RegistrationTokensController(FitnessDbContext context)
        {
            _context = context;
        }

        // GET: RegistrationTokens
        public async Task<IActionResult> Index()
        {
            var fitnessDbContext = _context.RegistrationTokens.Include(r => r.CreatedByUser).Include(r => r.UsedByUser);
            return View(await fitnessDbContext.ToListAsync());
        }

        // GET: RegistrationTokens/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationToken = await _context.RegistrationTokens
                .Include(r => r.CreatedByUser)
                .Include(r => r.UsedByUser)
                .FirstOrDefaultAsync(m => m.Token == id);
            if (registrationToken == null)
            {
                return NotFound();
            }

            return View(registrationToken);
        }

        // GET: RegistrationTokens/Create
        public IActionResult Create()
        {
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "DisplayName");
            ViewData["UsedByUserId"] = new SelectList(_context.Users, "Id", "DisplayName");
            return View();
        }

        // POST: RegistrationTokens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Token,CreatedByUserId,UsedByUserId,IsActive,CreatedAt,ExpiresAt,Description")] RegistrationToken registrationToken)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registrationToken);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "DisplayName", registrationToken.CreatedByUserId);
            ViewData["UsedByUserId"] = new SelectList(_context.Users, "Id", "DisplayName", registrationToken.UsedByUserId);
            return View(registrationToken);
        }

        // GET: RegistrationTokens/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationToken = await _context.RegistrationTokens.FindAsync(id);
            if (registrationToken == null)
            {
                return NotFound();
            }
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "DisplayName", registrationToken.CreatedByUserId);
            ViewData["UsedByUserId"] = new SelectList(_context.Users, "Id", "DisplayName", registrationToken.UsedByUserId);
            return View(registrationToken);
        }

        // POST: RegistrationTokens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Token,CreatedByUserId,UsedByUserId,IsActive,CreatedAt,ExpiresAt,Description")] RegistrationToken registrationToken)
        {
            if (id != registrationToken.Token)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registrationToken);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationTokenExists(registrationToken.Token))
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
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "DisplayName", registrationToken.CreatedByUserId);
            ViewData["UsedByUserId"] = new SelectList(_context.Users, "Id", "DisplayName", registrationToken.UsedByUserId);
            return View(registrationToken);
        }

        // GET: RegistrationTokens/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registrationToken = await _context.RegistrationTokens
                .Include(r => r.CreatedByUser)
                .Include(r => r.UsedByUser)
                .FirstOrDefaultAsync(m => m.Token == id);
            if (registrationToken == null)
            {
                return NotFound();
            }

            return View(registrationToken);
        }

        // POST: RegistrationTokens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var registrationToken = await _context.RegistrationTokens.FindAsync(id);
            if (registrationToken != null)
            {
                _context.RegistrationTokens.Remove(registrationToken);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationTokenExists(string id)
        {
            return _context.RegistrationTokens.Any(e => e.Token == id);
        }
    }
}
