using FitTrackApi.Domain.Entity;

namespace FitTrackApi.Application.Interfaces.RepositoryDI;

public interface IExerciseRepository : IRepository<Exercise, Guid>
{
    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Exercise>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}
