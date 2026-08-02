using FitTrackApi.Domain.Entity;

namespace FitTrackApi.Application.Interfaces.RepositoryDI;

public interface IBodyMetricRepository : IRepository<BodyMetric, Guid>
{
    Task<BodyMetric?> GetByUserIdAsync(string userId, CancellationToken ct = default);

}
