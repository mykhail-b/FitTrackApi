using FitTrackApi.Domain.Entity;

namespace FitTrackApi.Application.Interfaces.RepositoryDI;

public interface IFoodRepository : IRepository<Food, Guid>
{
    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Food>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}
