using Fitness.DataAccess;
using Fitness.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Fitness.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly FitnessDbContext _context;

        public AdminController(FitnessDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new AdminIndexViewModel();

            var tokens = await _context.RegistrationTokens
                .Include(r => r.CreatedByUser)
                .Include(r => r.UsedByUser)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            string baseUrl = $"{Request.Scheme}://{Request.Host.Value}";

            model.AdminIndexViewModelRegistrationTokens = tokens.Select(r => new AdminIndexViewModelRegistrationTokens
            {
                Token = r.Token,
                IsActive = r.IsActive,
                ExpiresAt = r.ExpiresAt,
                UsedByUser = r.UsedByUser?.DisplayName,
                Description = r.Description,
                Link = (r.IsActive && r.ExpiresAt > DateTime.UtcNow)
                        ? $"{baseUrl}/Account/Register/{r.Token}"
                        : null
            }).ToList();

            return View(model);
        }
    }
}
