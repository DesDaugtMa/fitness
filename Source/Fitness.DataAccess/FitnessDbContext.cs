using Fitness.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fitness.DataAccess
{
    public class FitnessDbContext : DbContext
    {
        public FitnessDbContext(DbContextOptions<FitnessDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RegistrationToken> RegistrationTokens { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<MuscleGroup> MuscleGroups { get; set; }
        public DbSet<ExerciseMuscleGroup> ExerciseMuscleGroups { get; set; }
        public DbSet<TrainingPlan> TrainingPlans { get; set; }
        public DbSet<TrainingDay> TrainingDays { get; set; }
        public DbSet<PlanExercise> PlanExercises { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<WorkoutLog> WorkoutLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enforce Unique constraints
            modelBuilder.Entity<Role>().HasIndex(r => r.Name).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.ProviderId).IsUnique();
            modelBuilder.Entity<MuscleGroup>().HasIndex(m => m.Name).IsUnique();

            // RegistrationToken PK
            modelBuilder.Entity<RegistrationToken>().HasKey(rt => rt.Token);

            // ExerciseMuscleGroups Composite PK
            modelBuilder.Entity<ExerciseMuscleGroup>()
                .HasKey(emg => new { emg.ExerciseId, emg.MuscleGroupId });

            // Configure User -> Friendship mapping to avoid Cascade Delete cycles
            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Requester)
                .WithMany(u => u.FriendshipsRequested)
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Addressee)
                .WithMany(u => u.FriendshipsReceived)
                .HasForeignKey(f => f.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ProfileImage)
                .WithMany() // Ein Bild hat in der Image-Klasse keine Property, die zurück zum Profil zeigt
                .HasForeignKey(u => u.ProfileImageId)
                .OnDelete(DeleteBehavior.SetNull); // Wenn das Bild gelöscht wird, verliert der User sein Profilbild (aber der User wird nicht gelöscht)

            // 2. Beziehung: Image -> Uploader
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Uploader)
                .WithMany() // Der User hat in der User-Klasse keine Liste von allen hochgeladenen Bildern (optional könnte man sie hinzufügen)
                .HasForeignKey(i => i.UploaderId)
                .OnDelete(DeleteBehavior.Restrict); // Verhindert zyklische Cascade-Delete Fehler, falls ein User gelöscht wird

            modelBuilder.Entity<WorkoutSession>()
                .HasOne(ws => ws.User)
                .WithMany()
                .HasForeignKey(ws => ws.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Kein automatisches Löschen der Session durch den User (wird durch Plan geregelt)

            // 2. LÖSUNG FÜR WorkoutLogs (Proaktiv, da dieser Fehler als nächstes käme):
            // Verhindert den Konflikt zwischen WorkoutSession -> WorkoutLog und PlanExercise -> WorkoutLog
            modelBuilder.Entity<WorkoutLog>()
                .HasOne(wl => wl.PlanExercise)
                .WithMany()
                .HasForeignKey(wl => wl.PlanExerciseId)
                .OnDelete(DeleteBehavior.Restrict); // Kein automatisches Löschen der Logs durch die PlanExercise (wird durch Session geregelt)

            // 3. LÖSUNG FÜR Exercise -> User (Creator):
            // Zur Sicherheit, falls ein User gelöscht wird, sollen seine erstellten (vielleicht öffentlichen) Übungen nicht verschwinden
            modelBuilder.Entity<Exercise>()
                .HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.SetNull); // Die Übung bleibt erhalten, hat aber keinen Creator mehr

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User" },
                new Role { Id = 2, Name = "Admin" }
            );

            modelBuilder.Entity<MuscleGroup>().HasData(
                new MuscleGroup { Id = 1, Name = "Brust" },
                new MuscleGroup { Id = 2, Name = "Rücken" },
                new MuscleGroup { Id = 3, Name = "Beine" },
                new MuscleGroup { Id = 4, Name = "Schultern" },
                new MuscleGroup { Id = 5, Name = "Bizeps" },
                new MuscleGroup { Id = 6, Name = "Trizeps" }
            );

            modelBuilder.Entity<RegistrationToken>().HasData(
                new RegistrationToken
                {
                    Token = "INITIAL_SETUP_TOKEN",
                    CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                    IsActive = true,
                }
            );
        }
    }
}
