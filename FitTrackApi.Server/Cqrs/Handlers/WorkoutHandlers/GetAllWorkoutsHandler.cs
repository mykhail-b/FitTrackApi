using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using FitTrackApi.Core.Dto.Workout;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.WorkoutHandlers;

public record GetAllUserWorkoutsQuery(string UserId);

public class GetAllUserWorkoutsHandler : IQueryHandler<GetAllUserWorkoutsQuery, List<WorkoutDto>>
{
    private readonly DataContext _dbContext;

    public GetAllUserWorkoutsHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<WorkoutDto>> Handle(GetAllUserWorkoutsQuery query, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Workouts
            .AsNoTracking()
            .Where(w => w.UserId == query.UserId)
            .Select(w => new WorkoutDto
            {
                Id = w.Id,
                Date = w.Date,
                Notes = w.Notes,
                Exercises = w.Exercises.Select(e => new WorkoutExerciseDto
                {
                    ExerciseId = e.ExerciseId,
                    Sets = e.Sets,
                    Reps = e.Reps,
                    Weight = e.Weight
                }).ToList()
            })
            .ToListAsync(cancellationToken);
    }
}