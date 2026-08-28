using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Repository;

/// <summary>
/// 
/// </summary>
public class WorkoutRepository : Repository<Workout, Guid>, IWorkoutRepository
{
    public WorkoutRepository(DataContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Workout>> GetAllForUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.Date)
            .ToListAsync(cancellationToken);
    }

    public async Task<Workout?> GetByIdWithExercisesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .Include(w => w.Sets)
            .ThenInclude(we => we.Exercise)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<List<DateOnly>> GetWorkoutActivityAsync(string userId, CancellationToken cancellationToken = default)
    {
        var dates = await DbSet.AsNoTracking()
            .Where(w => w.UserId == userId)
            .Select(w => w.Date)
            .ToListAsync(cancellationToken);

        return dates.Select(d => DateOnly.FromDateTime(d))
            .Distinct()
            .OrderBy(d => d)
            .ToList();
    }

    public async Task<int> CountAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(w => w.UserId == userId)
            .CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Workout>> GetPagedAsync(string userId, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var skip = (pageNumber - 1) * pageSize;

        return await DbSet
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.Date)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
