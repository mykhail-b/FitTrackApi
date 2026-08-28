using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Repository;

public class ExerciseRepository : Repository<Exercise, Guid>, IExerciseRepository
{
    public ExerciseRepository(DataContext context) : base(context) { }

    public async Task<int> CountAsync(CancellationToken ct = default)
    {
        return await DbSet.CountAsync(ct);
    }

    public async Task<IReadOnlyList<Exercise>> GetAllAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var skip = (pageNumber - 1) * pageSize;

        return await DbSet
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);
    }
}