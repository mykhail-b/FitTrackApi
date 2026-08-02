using FitTrackApi.Application.Interfaces;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Repository;

public class ExerciseRepository : Repository<Exercise, int>, IExerciseRepository
{
    public ExerciseRepository(DataContext context) : base(context) { }

    public async Task<int> CountAsync(CancellationToken ct = default)
        => await DbSet.CountAsync(ct);


    // Legacy overload, keep for compatibility: return null for Guid-based calls
    public Task<Exercise?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult<Exercise?>(null);

    public async Task<IReadOnlyList<Exercise>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var skip = (pageNumber - 1) * pageSize;
        return await DbSet.AsNoTracking()
            .OrderBy(e => e.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
    }
}
