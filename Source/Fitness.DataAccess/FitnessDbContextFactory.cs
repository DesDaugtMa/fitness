using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Fitness.DataAccess // <-- Prüfe, ob dein Namespace exakt so heißt
{
    // Diese Klasse wird NUR von EF Core Tools (Migrations, Bundles) aufgerufen
    public class FitnessDbContextFactory : IDesignTimeDbContextFactory<FitnessDbContext>
    {
        public FitnessDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FitnessDbContext>();

            // WICHTIG: Das ist nur ein Dummy-String für den C#-Compiler!
            // Wenn das Bundle in deiner Pipeline mit --connection aufgerufen wird, 
            // wird dieser String automatisch durch dein GitHub Secret überschrieben.
            optionsBuilder.UseSqlServer("Server=localhost;Database=EFDummy;Trusted_Connection=True;TrustServerCertificate=True;");

            return new FitnessDbContext(optionsBuilder.Options);
        }
    }
}