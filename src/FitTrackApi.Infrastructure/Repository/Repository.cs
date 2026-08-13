using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Application.Interfaces;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    protected readonly DataContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(DataContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default)
        => await DbSet.FindAsync(new object?[] { id }, ct);

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
        => await DbSet.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(TEntity entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    public async Task UpdateAsync(TEntity entity) => DbSet.Update(entity);

    public async Task RemoveAsync(TEntity entity) => DbSet.Remove(entity);
}
