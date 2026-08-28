using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Repository;

/// <summary>
/// 
/// </summary>
public class FoodRepository : Repository<Food, Guid>, IFoodRepository
{
    public FoodRepository(DataContext context) : base(context) { }

    public async Task<int> CountAsync(CancellationToken ct = default)
    {
        return await DbSet.CountAsync(ct);
    }

    public async Task<IReadOnlyList<Food>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var skip = (pageNumber - 1) * pageSize;

        return await DbSet
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
    }
}
