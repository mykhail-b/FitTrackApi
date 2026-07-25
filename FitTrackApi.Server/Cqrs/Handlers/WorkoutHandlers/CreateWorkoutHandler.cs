using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using FitTrackApi.Core.Dto.Workout;
using FitTrackApi.Core.Entity;

namespace FitTrackApi.Server.Cqrs.Handlers.WorkoutHandlers;

public record CreateWorkoutCommand(string UserId, DateTime Date, string? Notes, List<WorkoutExerciseDto> Exercises);

public class CreateWorkoutHandler : ICommandHandler<CreateWorkoutCommand, bool>
{
    private readonly DataContext _dbContext;

    public CreateWorkoutHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(CreateWorkoutCommand command, CancellationToken cancellationToken = default)
    {
        var workout = new Workout
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            Date = command.Date,
            Notes = command.Notes
        };

        foreach (var ex in command.Exercises)
        {
            workout.Exercises.Add(new WorkoutExercise
            {
                Id = Guid.NewGuid(),
                WorkoutId = workout.Id,
                Workout = workout,
                ExerciseId = ex.ExerciseId,
                Sets = ex.Sets,
                Reps = ex.Reps,
                Weight = ex.Weight
            });
        }

        _dbContext.Workouts.Add(workout);
        var affected = await _dbContext.SaveChangesAsync(cancellationToken);

        return affected > 0;
    }
}