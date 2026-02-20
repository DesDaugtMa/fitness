using Fitness.Config;
using Fitness.DataAccess;
using Fitness.DataAccess.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fitness
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            IHostBuilder hostBuilder = builder.Host.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.Sources.Clear();
                configurationBuilder.AddJsonFile($"Config/appsettings.json", false);
                configurationBuilder.AddJsonFile($"Config/appsettings.{builder.Environment.EnvironmentName}.json", true);
                configurationBuilder.AddEnvironmentVariables();
                configurationBuilder.AddUserSecrets<Program>(true);
            });

            AppSettings appSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

            builder.Services.AddDbContext<FitnessDbContext>(options =>
                options.UseSqlServer(appSettings!.ConnectionStrings!.Default));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.SlidingExpiration = true;
            })
            .AddGoogle(options =>
            {
                options.ClientId = appSettings!.GoogleAuthSettings!.ClientId!;
                options.ClientSecret = appSettings!.GoogleAuthSettings!.ClientSecret!;
            });

            // --- NEU: PasswordHasher als Singleton registrieren ---
            builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
