using FitTrackApi.Application.Interfaces;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Repository;

public class WorkoutRepository : Repository<Workout, Guid>, IWorkoutRepository
{
    public WorkoutRepository(DataContext context) : base(context) { }

    public async Task<IReadOnlyList<Workout>> GetAllForUserAsync(string userId, CancellationToken ct = default)
        => await DbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.Date)
            .ToListAsync(ct);

    public async Task<Workout?> GetByIdWithExercisesAsync(Guid id, CancellationToken ct = default)
        => await DbSet
            .Include(w => w.Exercises)
                .ThenInclude(we => we.Exercise)
            .FirstOrDefaultAsync(w => w.Id == id, ct);

    public async Task<List<DateOnly>> GetWorkoutActivityAsync(string userId, CancellationToken ct = default)
    {
        var dates = await DbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .Select(w => w.Date)
            .ToListAsync(ct);

        return dates
            .Select(d => DateOnly.FromDateTime(d))
            .Distinct()
            .OrderBy(d => d)
            .ToList();
    }

    public async Task ReplaceExercisesAsync(Workout workout, ICollection<WorkoutExercise> newExercises, CancellationToken ct = default)
    {
        // Remove existing exercises for the workout
        var existing = Context.Set<WorkoutExercise>().Where(we => we.WorkoutId == workout.Id);
        Context.Set<WorkoutExercise>().RemoveRange(existing);

        // Add new exercises (ensure WorkoutId is set)
        foreach (var ex in newExercises)
        {
            ex.WorkoutId = workout.Id;
            // clear navigation to avoid confusion
            ex.Workout = null;
            Context.Set<WorkoutExercise>().Add(ex);
        }

        await Task.CompletedTask;
    }
}
