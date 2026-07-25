using FitTrackApi.Core.Dto.Exercise;
using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.ExerciseHandlers;

public record GetExercisesPagedQuery(int PageNumber = 1, int PageSize = 10);

public class GetExercisesPagedHandler : IQueryHandler<GetExercisesPagedQuery, PagedListResult<ExerciseListItemResult>>
{
    private readonly DataContext _dbContext;

    public GetExercisesPagedHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedListResult<ExerciseListItemResult>> Handle(GetExercisesPagedQuery query, CancellationToken cancellationToken = default)
    {
        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var totalCount = await _dbContext.Exercises.CountAsync(cancellationToken);

        var items = await _dbContext.Exercises
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new ExerciseListItemResult
            {
                Id = e.Id,
                Name = e.Name,
                Force = e.Force,
                Level = e.Level,
                Mechanic = e.Mechanic,
                Equipment = e.Equipment,
                Image = e.Images.FirstOrDefault() ?? string.Empty
            })
            .ToListAsync(cancellationToken);

        return new PagedListResult<ExerciseListItemResult>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}