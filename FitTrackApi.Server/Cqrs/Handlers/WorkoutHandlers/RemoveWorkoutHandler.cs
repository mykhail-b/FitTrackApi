using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.WorkoutHandlers;

public record RemoveWorkoutCommand(Guid WorkoutId);

public class RemoveWorkoutHandler : ICommandHandler<RemoveWorkoutCommand, bool>
{
    private readonly DataContext _dbContext;

    public RemoveWorkoutHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(RemoveWorkoutCommand command, CancellationToken cancellationToken = default)
    {
        var workout = await _dbContext.Workouts
            .FirstOrDefaultAsync(w => w.Id == command.WorkoutId, cancellationToken);

        if (workout is null)
            return false;

        _dbContext.Workouts.Remove(workout);
        var affected = await _dbContext.SaveChangesAsync(cancellationToken);

        return affected > 0;
    }
}