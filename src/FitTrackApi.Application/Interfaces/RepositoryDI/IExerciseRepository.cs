using FitTrackApi.Domain.Entity;

namespace FitTrackApi.Application.Interfaces.RepositoryDI;

public interface IExerciseRepository : IRepository<Exercise, int>
{
    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Exercise>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}
