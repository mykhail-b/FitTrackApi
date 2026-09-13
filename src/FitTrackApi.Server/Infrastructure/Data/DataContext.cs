using FitTrackApi.Domain.Entity;
using FitTrackApi.Server.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Data;

public class DataContext : IdentityDbContext<IdentityUser>
{
    public DataContext(DbContextOptions<DataContext> options)
    : base(options)
    {
    }



    public DbSet<Account> Accounts { get; set; }

    public DbSet<Workout> Workouts { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Meal> Meals { get; set; }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .IsRequired();

        modelBuilder.Entity<Workout>()
            .HasOne(e => e.Account)
            .WithMany(a => a.Workouts)
            .HasForeignKey(w => w.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Account → Meals (1:N)
        modelBuilder.Entity<Meal>()
            .HasOne(e => e.Account)
            .WithMany(a => a.Meals)
            .HasForeignKey(m => m.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<Workout>()
            .HasMany(e => e.WorkoutSets)
            .WithOne(e => e.Workout)
            .HasForeignKey(e => e.WorkoutId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkoutSet>()
            .HasOne(e => e.Exercises)
            .WithMany()
            .HasForeignKey(e => e.ExerciseId)
            .IsRequired();
    }
}