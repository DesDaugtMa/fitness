using Fitness.DataAccess;
using Fitness.DataAccess.Models;
using Fitness.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Fitness.Controllers
{
    public class AccountController : Controller
    {
        private readonly FitnessDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(FitnessDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Register(string id)
        {
            RegistrationToken? registrationToken = _context.RegistrationTokens.FirstOrDefault(rt => rt.Token == id);

            if (registrationToken is null || !registrationToken.IsActive || registrationToken.ExpiresAt < DateTime.UtcNow)
                return RedirectToAction("Login", "Account");

            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(new RegisterViewModel { RegistrationToken = id });
        }

        // --- POST: Registrierung ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Prüfen ob E-Mail schon existiert
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Diese E-Mail-Adresse wird bereits verwendet.");
                    return View(model);
                }

                // Standard-Rolle suchen (z.B. "User"). Du musst sicherstellen, dass diese in der DB existiert!
                var defaultRole = _context.Roles.FirstOrDefault(r => r.Name == "User");
                if (defaultRole == null)
                {
                    // Fallback, falls die Rolle nicht existiert. In einer echten App via Data-Seeding lösen.
                    defaultRole = new Role { Name = "User" };
                    _context.Roles.Add(defaultRole);
                    await _context.SaveChangesAsync();
                }

                var user = new User
                {
                    Email = model.Email,
                    DisplayName = model.DisplayName,
                    RoleId = defaultRole.Id,
                    CreatedAt = DateTime.UtcNow
                };

                // Passwort hashen
                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                _context.RegistrationTokens
                    .Where(rt => rt.Token == model.RegistrationToken)
                    .ExecuteUpdate(rt => rt
                        .SetProperty(r => r.IsActive, false)
                        .SetProperty(r => r.UsedByUserId, user.Id)
                    );

                // 1. Claims erstellen (Informationen, die im Cookie gespeichert werden)
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.DisplayName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.Role.Name) // Rollen-Zuweisung!
                        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
                };

                // 2. Anmelden (Cookie setzen)
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // --- GET: Login ---
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return LocalRedirect(returnUrl);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // --- POST: Login ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // User mitsamt Rolle laden, um die Claims zu befüllen
                var user = _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.Email == model.Email);

                if (user != null && user.PasswordHash != null)
                {
                    // Passwort verifizieren
                    var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        // 1. Claims erstellen (Informationen, die im Cookie gespeichert werden)
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.DisplayName),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Role, user.Role.Name) // Rollen-Zuweisung!
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
                        };

                        // 2. Anmelden (Cookie setzen)
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return LocalRedirect(returnUrl);
                    }
                }

                ModelState.AddModelError(string.Empty, "Ungültiger Login-Versuch.");
            }

            return View(model);
        }

        // --- POST: Logout ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
