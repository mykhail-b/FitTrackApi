using FitTrackApi.Core.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Data;

public class DataContext : IdentityDbContext<UserAccount>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected DataContext()
    {
    }

    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<BodyMetric> BodyMetrics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // Exercise
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PrimaryMuscles).HasStringListConversion();
            entity.Property(e => e.SecondaryMuscles).HasStringListConversion();
            entity.Property(e => e.Instructions).HasStringListConversion();
            entity.Property(e => e.Images).HasStringListConversion();
        });

        // BodyMetric -> UserAccount (1 : many)
        modelBuilder.Entity<BodyMetric>(entity =>
        {
            entity.HasKey(d => d.Id);

            entity.HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Workout -> UserAccount (1 : many)
        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(w => w.Id);

            entity.HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // WorkoutExercise -> Workout (many : 1)
        // WorkoutExercise -> Exercise (many : 1)
        modelBuilder.Entity<WorkoutExercise>(entity =>
        {
            entity.HasKey(we => we.Id);

            entity.HasOne(we => we.Workout)
                .WithMany(w => w.Exercises)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(we => we.Exercise)
    .WithMany()
    .HasForeignKey(we => we.ExerciseId)
    .OnDelete(DeleteBehavior.Restrict);

            entity.Property(we => we.Weight)
                .HasPrecision(10, 2);
        });
    }


}