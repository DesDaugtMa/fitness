using Fitness.Config;
using Fitness.DataAccess;
using Fitness.DataAccess.Models;
using Fitness.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Fitness.Controllers
{
    public class AccountController : Controller
    {
        private readonly FitnessDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AppSettings _appSettings;

        public AccountController(FitnessDbContext context, IPasswordHasher<User> passwordHasher, AppSettings appSettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _appSettings = appSettings;
        }

        [HttpGet("Account/Register/{id?}")]
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoogleLogin(string token)
        {
            // Wir speichern den Registrierungstoken im AuthenticationProperties, 
            // damit er nach dem Google-Redirect wieder verfügbar ist.
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse"),
                Items = { { "RegistrationToken", token } }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {

            // 1. Authentifizierung prüfen und Daten auslesen
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded) return RedirectToAction("Login");

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            result.Properties.Items.TryGetValue("RegistrationToken", out string? tokenValue);

            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login");

            // 2. Datenbankabfragen: Bestehenden User und Token (falls vorhanden) laden
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            var passedToken = await _context.RegistrationTokens.FirstOrDefaultAsync(rt => rt.Token == tokenValue);

            bool allowLogin = false;

            // --- 3. LOGIK-ENTSCHEIDUNGEN ---

            // Fall 1: KEIN Token übergeben -> Darf der User trotzdem rein?
            if (string.IsNullOrEmpty(tokenValue))
            {
                if (user != null)
                {
                    // Prüfen, ob die E-Mail/der User an einen bereits benutzten Token gebunden ist
                    bool isLinkedToToken = await _context.RegistrationTokens.AnyAsync(rt => rt.UsedByUserId == user.Id);
                    if (isLinkedToToken)
                    {
                        allowLogin = true; // Anmelden erlaubt
                    }
                }
            }
            // Fall 2 & 3: EIN Token wurde übergeben -> Ist er gültig?
            else if (passedToken != null && passedToken.IsActive)
            {
                if (user == null)
                {
                    // Fall 2: Gültiger Token, aber NEUER User -> Registrieren
                    user = new User
                    {
                        Email = email,
                        DisplayName = name ?? email,
                        PasswordHash = "EXTERNAL_AUTH", // Kein Passwort bei Google-Auth
                        RoleId = 2,                     // Standardrolle
                        AuthProvider = "Google"
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();  // Speichern, um die neue user.Id zu generieren
                }

                // Fall 3 (implizit): Gültiger Token, und User existiert bereits
                // (Wird automatisch abgehandelt, da 'user' hier nicht null ist)

                // Token entwerten (gilt für Registrierung UND bestehenden User mit neuem Token)
                passedToken.IsActive = false;
                passedToken.UsedByUserId = user.Id;
                await _context.SaveChangesAsync();

                allowLogin = true; // Anmelden erlaubt
            }

            // --- 4. ABSCHLUSS ---

            // Wenn keine der Bedingungen zutrifft (z.B. ungültiger Token, User existiert nicht)
            if (!allowLogin)
            {
                return RedirectToAction("AccessDenied", "Account"); // Oder eine andere Fehlerseite
            }

            // Login durchführen
            // ... (Deine bestehende SignInAsync-Logik mit Claims hier einfügen)

            return RedirectToAction("Index", "Home");
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
