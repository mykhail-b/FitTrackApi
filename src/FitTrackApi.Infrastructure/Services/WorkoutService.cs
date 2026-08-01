
using FitTrackApi.Application.Dto;
using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Infrastructure.Entity;
using FitTrackApi.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Services;

public interface IWorkoutService
{
    Task<WorkoutDto?> GetByIdAsync(Guid workoutId, CancellationToken ct = default);
    Task<List<WorkoutDto>> GetAllForUserAsync(string userId, CancellationToken ct = default);
    Task CreateAsync(string userId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default);
    Task UpdateAsync(Guid workoutId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default);
    Task <List<DateOnly>>GetWorkoutActivity(string userId, CancellationToken ct = default);
    Task <bool> RemoveAsync(Guid workoutId, CancellationToken ct = default);
}

public class WorkoutService : IWorkoutService
{
    private readonly DataContext _dbContext;
    public WorkoutService(DataContext dbContext) => _dbContext = dbContext;

    /// <summary>
    /// Mapper delegate to convert Workout entity to WorkoutDto
    /// </summary>
    /// <param name="w"></param>
    /// <returns></returns>
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

    public async Task CreateAsync(string userId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default)
    {
        var workout = new Workout { Id = Guid.NewGuid(), UserId = userId, Date = date, Notes = notes };
        workout.Exercises = WorkoutExerciseFactory.BuildFrom(workout.Id, workout, exercises);

        _dbContext.Workouts.Add(workout);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Guid workoutId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default)
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

        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<List<DateOnly>> GetWorkoutActivity(string userId, CancellationToken ct = default)
    {
        var activeDays = await _dbContext.Workouts
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .Select(w => DateOnly.FromDateTime(w.Date))
            .Distinct()
            .OrderBy(d => d)
            .ToListAsync(ct);

        return activeDays;
    }

    public async Task <bool> RemoveAsync(Guid workoutId, CancellationToken ct = default)
    {
        var workout = await _dbContext.Workouts.FirstOrDefaultAsync(w => w.Id == workoutId, ct);
        if (workout is null)
            return false;

        _dbContext.Workouts.Remove(workout);
        await _dbContext.SaveChangesAsync(ct);

        return true;
    }
}