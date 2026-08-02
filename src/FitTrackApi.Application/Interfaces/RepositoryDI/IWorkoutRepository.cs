namespace FitTrackApi.Application.Interfaces.RepositoryDI;

using FitTrackApi.Domain.Entity;

public interface IWorkoutRepository : IRepository<Workout, Guid>
{
    Task<Workout?> GetByIdWithExercisesAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Workout>> GetAllForUserAsync(string userId, CancellationToken ct = default);
    Task<List<DateOnly>> GetWorkoutActivityAsync(string userId, CancellationToken ct = default);
    Task ReplaceExercisesAsync(Workout workout, ICollection<WorkoutExercise> newExercises, CancellationToken ct = default);
}