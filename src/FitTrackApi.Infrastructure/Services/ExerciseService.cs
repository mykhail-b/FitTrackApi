using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Services;
public interface IExerciseService
{
    Task<ExerciseDetailsResult?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedListResult<ExerciseListItem>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}

public class ExerciseService : IExerciseService
{
    private readonly DataContext _dbContext;
    public ExerciseService(DataContext dbContext) => _dbContext = dbContext;

    public async Task<ExerciseDetailsResult?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbContext.Exercises
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new ExerciseDetailsResult
            {
                Id = e.Id,
                Name = e.Name,
                Force = e.Force,
                Level = e.Level,
                Mechanic = e.Mechanic,
                Equipment = e.Equipment,
                PrimaryMuscles = e.PrimaryMuscles,
                SecondaryMuscles = e.SecondaryMuscles,
                Instructions = e.Instructions,
                Category = e.Category,
                Images = e.Images
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PagedListResult<ExerciseListItem>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var totalCount = await _dbContext.Exercises.CountAsync(ct);

        var items = await _dbContext.Exercises
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ExerciseListItem
            {
                Id = e.Id,
                Name = e.Name,
                Image = e.Images.FirstOrDefault() ?? string.Empty
            })
            .ToListAsync(ct);

        return new PagedListResult<ExerciseListItem>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}