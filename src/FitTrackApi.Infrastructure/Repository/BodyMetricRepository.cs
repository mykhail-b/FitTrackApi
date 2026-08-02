using FitTrackApi.Application.Interfaces;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Repository;

public class BodyMetricRepository : Repository<BodyMetric, Guid>, IBodyMetricRepository
{
    public BodyMetricRepository(DataContext context) : base(context) { }

    public async Task<BodyMetric?> GetByUserIdAsync(string userId, CancellationToken ct = default)
        => await DbSet
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.UpdatedAt)
            .FirstOrDefaultAsync(ct);
}
