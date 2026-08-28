using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.IdentityEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Data;

public class DataContext : IdentityDbContext<UserAccount>
{
    public DataContext(DbContextOptions<DataContext> options)
    : base(options)
    {
    }

    //protected DataContext()
    //{
    //}

    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<Food> Foods { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Exercise
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Category).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Force).HasMaxLength(50);
            entity.Property(e => e.Mechanic).HasMaxLength(50);
            entity.Property(e => e.Equipment).HasMaxLength(100);
            entity.Property(e => e.MeasurabilityType).HasMaxLength(50);

            entity.HasIndex(e => e.Name);

            entity.Property(e => e.Muscles).HasStringListConversion();
            entity.Property(e => e.Instructions).HasMaxLength(200);
            entity.Property(e => e.Images).HasMaxLength(200);
        });

        // Workout -> UserAccount (1 : many), without navigation property in Domain
        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.UserId).IsRequired();
            entity.Property(w => w.Notes).HasMaxLength(1000);

            entity.HasOne<UserAccount>()
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(w => new { w.UserId, w.Date });
        });

        // WorkoutExercise -> Workout (many : 1)
        // WorkoutExercise -> Exercise (many : 1)
        modelBuilder.Entity<WorkoutSet>(entity =>
        {
            entity.HasKey(we => we.Id);

            entity.HasOne(we => we.Workout)
                .WithMany(w => w.Sets)
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