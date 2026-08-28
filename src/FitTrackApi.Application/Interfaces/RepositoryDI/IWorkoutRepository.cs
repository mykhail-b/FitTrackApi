namespace FitTrackApi.Application.Interfaces.RepositoryDI;

using FitTrackApi.Domain.Entity;

public interface IWorkoutRepository : IRepository<Workout, Guid>
{
    Task<IReadOnlyList<Workout>> GetAllForUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<Workout?> GetByIdWithExercisesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<DateOnly>> GetWorkoutActivityAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Workout>> GetPagedAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string userId, CancellationToken cancellationToken = default);
}