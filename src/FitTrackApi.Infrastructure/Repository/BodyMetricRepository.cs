using FitTrackApi.Application.Interfaces;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Repository;

public class BodyMetricRepository : Repository<BodyMetric, Guid>, IBodyMetricRepository
{
    public BodyMetricRepository(DataContext context) : base(context) { }

    public async Task AddAsync(BodyMetric entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    public async Task<IReadOnlyList<BodyMetric>> GetAllAsync(CancellationToken ct = default)
        => await DbSet.AsNoTracking().ToListAsync(ct);

    public async Task<BodyMetric?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync(new object[] { id }, ct);

    public async Task<BodyMetric?> GetByUserIdAsync(string userId, CancellationToken ct = default)
        => await DbSet
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.UpdatedAt)
            .FirstOrDefaultAsync(ct);

    public void Remove(BodyMetric entity) => DbSet.Remove(entity);

    public void Update(BodyMetric entity) => DbSet.Update(entity);
}
