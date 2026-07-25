using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using FitTrackApi.Core.Dto.Workout;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.WorkoutHandlers;

public record GetWorkoutByIdQuery(Guid WorkoutId);

public class GetWorkoutByIdHandler : IQueryHandler<GetWorkoutByIdQuery, WorkoutDto?>
{
    private readonly DataContext _dbContext;

    public GetWorkoutByIdHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WorkoutDto?> Handle(GetWorkoutByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Workouts
            .AsNoTracking()
            .Where(w => w.Id == query.WorkoutId)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}