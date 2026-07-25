using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using FitTrackApi.Core.Dto.Workout;
using FitTrackApi.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.WorkoutHandlers;

public record UpdateWorkoutCommand(Guid WorkoutId, DateTime Date, string? Notes, List<WorkoutExerciseDto> Exercises);

public class UpdateWorkoutHandler : ICommandHandler<UpdateWorkoutCommand, bool>
{
    private readonly DataContext _dbContext;

    public UpdateWorkoutHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateWorkoutCommand command, CancellationToken cancellationToken = default)
    {
        var workout = await _dbContext.Workouts
            .Include(w => w.Exercises)
            .FirstOrDefaultAsync(w => w.Id == command.WorkoutId, cancellationToken);

        if (workout is null)
            throw new KeyNotFoundException($"Workout {command.WorkoutId} not found");

        workout.Date = command.Date;
        workout.Notes = command.Notes;

        _dbContext.RemoveRange(workout.Exercises);
        workout.Exercises.Clear();

        foreach (var ex in command.Exercises)
        {
            workout.Exercises.Add(new WorkoutExercise
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout.Id,
                Workout = workout,
                ExerciseId = ex.ExerciseId,
                Exercise = null!,
                Sets = ex.Sets,
                Reps = ex.Reps,
                Weight = ex.Weight
            });
        }

        var affected = await _dbContext.SaveChangesAsync(cancellationToken);
        return affected > 0;
    }
}