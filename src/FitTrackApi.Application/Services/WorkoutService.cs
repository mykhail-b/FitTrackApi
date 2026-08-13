using FitTrackApi.Application.Dto.Workout;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using FitTrackApi.Domain.Entity;

namespace FitTrackApi.Application.Services;

public interface IWorkoutService
{
    Task<WorkoutDto?> GetByIdAsync(Guid workoutId, CancellationToken ct = default);
    Task<List<WorkoutDto>> GetAllForUserAsync(string userId, CancellationToken ct = default);
    Task CreateAsync(string userId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default);
    Task UpdateAsync(Guid workoutId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default);
    Task<List<DateOnly>> GetWorkoutActivityAsync(string userId, CancellationToken ct = default);
    Task<bool> RemoveAsync(Guid workoutId, CancellationToken ct = default);
}

public class WorkoutService : IWorkoutService
{
    private readonly IUnitOfWork _unitOfWork;
    public WorkoutService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

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
        var workout = await _unitOfWork.Workouts.GetByIdWithExercisesAsync(workoutId, ct);
        return workout is null ? null : ToDto(workout);
    }

    public async Task<List<WorkoutDto>> GetAllForUserAsync(string userId, CancellationToken ct = default)
    {
        var workouts = await _unitOfWork.Workouts.GetAllForUserAsync(userId, ct);
        return workouts.Select(ToDto).ToList();
    }

    public async Task CreateAsync(string userId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default)
    {
        var workout = new Workout { Id = Guid.NewGuid(), UserId = userId, Date = date, Notes = notes };
        workout.Exercises = WorkoutExerciseFactory.BuildFrom(workout.Id, workout, exercises);

        await _unitOfWork.Workouts.AddAsync(workout, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Guid workoutId, DateTime date, string? notes, List<WorkoutExerciseDto> exercises, CancellationToken ct = default)
    {
        var workout = await _unitOfWork.Workouts.GetByIdWithExercisesAsync(workoutId, ct)
            ?? throw new KeyNotFoundException($"Workout {workoutId} not found");

        workout.Date = date;
        workout.Notes = notes;

        // Ensure the modified workout is tracked so changes are persisted
        await _unitOfWork.Workouts.UpdateAsync(workout);

        var newExercises = WorkoutExerciseFactory.BuildFrom(workout.Id, workout, exercises);
        await _unitOfWork.Workouts.ReplaceExercisesAsync(workout, newExercises, ct);

        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<List<DateOnly>> GetWorkoutActivityAsync(string userId, CancellationToken ct = default)
        => await _unitOfWork.Workouts.GetWorkoutActivityAsync(userId, ct);

    public async Task<bool> RemoveAsync(Guid workoutId, CancellationToken ct = default)
    {
        var workout = await _unitOfWork.Workouts.GetByIdAsync(workoutId, ct);
        if (workout is null) return false;

        await _unitOfWork.Workouts.RemoveAsync(workout);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}