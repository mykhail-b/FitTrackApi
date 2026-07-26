using FitTrackApi.Core;
using FitTrackApi.Core.Dto.Workout;
using FitTrackApi.Core.Entity;
using FitTrackApi.Server.Data;
using Microsoft.EntityFrameworkCore;

public interface IWorkoutService
{
    Task<WorkoutDto?> GetByIdAsync(Guid workoutId, CancellationToken ct = default);
    Task<List<WorkoutDto>> GetAllForUserAsync(string userId, CancellationToken ct = default);
    Task<bool> CreateAsync(string userId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid workoutId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default);
    Task<bool> RemoveAsync(Guid workoutId, CancellationToken ct = default);
}

public class WorkoutService : IWorkoutService
{
    private readonly DataContext _dbContext;
    public WorkoutService(DataContext dbContext) => _dbContext = dbContext;

    private static WorkoutDto ToDto(Workout w) => new()
    {
        Id = w.Id,
        Date = w.Date,
        Notes = w.Notes,
        Exercises = w.Exercises.Select(e => new WorkoutExerciseDto
        {
            ExerciseId = e.ExerciseId,
            Sets = e.Sets,
            Reps = e.Reps,
            Weight = e.Weight
        }).ToList()
    };

    public async Task<WorkoutDto?> GetByIdAsync(Guid workoutId, CancellationToken ct = default)
    {
        var workout = await _dbContext.Workouts
            .AsNoTracking()
            .Include(w => w.Exercises)
            .FirstOrDefaultAsync(w => w.Id == workoutId, ct);

        return workout is null ? null : ToDto(workout);
    }

    public async Task<List<WorkoutDto>> GetAllForUserAsync(string userId, CancellationToken ct = default)
    {
        var workouts = await _dbContext.Workouts
            .AsNoTracking()
            .Include(w => w.Exercises)
            .Where(w => w.UserId == userId)
            .ToListAsync(ct);

        return workouts.Select(ToDto).ToList();
    }

    public async Task<bool> CreateAsync(string userId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default)
    {
        var workout = new Workout { Id = Guid.NewGuid(), UserId = userId, Date = date, Notes = notes };
        workout.Exercises = WorkoutExerciseFactory.BuildFrom(workout.Id, workout, exercises);

        _dbContext.Workouts.Add(workout);
        var affected = await _dbContext.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<bool> UpdateAsync(Guid workoutId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default)
    {
        var workout = await _dbContext.Workouts
            .Include(w => w.Exercises)
            .FirstOrDefaultAsync(w => w.Id == workoutId, ct)
            ?? throw new KeyNotFoundException($"Workout {workoutId} not found");

        workout.Date = date;
        workout.Notes = notes;

        _dbContext.RemoveRange(workout.Exercises);
        workout.Exercises.Clear();
        workout.Exercises = WorkoutExerciseFactory.BuildFrom(workout.Id, workout, exercises);

        var affected = await _dbContext.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<bool> RemoveAsync(Guid workoutId, CancellationToken ct = default)
    {
        var workout = await _dbContext.Workouts.FirstOrDefaultAsync(w => w.Id == workoutId, ct);
        if (workout is null) return false;

        _dbContext.Workouts.Remove(workout);
        var affected = await _dbContext.SaveChangesAsync(ct);
        return affected > 0;
    }
}