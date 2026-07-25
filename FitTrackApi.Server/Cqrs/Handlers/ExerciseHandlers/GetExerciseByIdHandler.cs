using FitTrackApi.Core.Dto.Exercise;
using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.ExerciseHandlers;

public record GetExerciseByIdQuery(int Id);

public class GetExerciseByIdHandler : IQueryHandler<GetExerciseByIdQuery, ExerciseDetailsResult?>
{
    private readonly DataContext _dbContext;

    public GetExerciseByIdHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExerciseDetailsResult?> Handle(GetExerciseByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Exercises
            .AsNoTracking()
            .Where(e => e.Id == query.Id)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}