using FitTrackApi.Application.Interfaces.RepositoryDI;
using MediatR;

namespace FitTrackApi.Application.Feature.Workouts.Commands;

public record DeleteWorkoutCommand(Guid WorkoutId, string UserId) : IRequest<bool>;

internal class DeleteWorkoutHandler : IRequestHandler<DeleteWorkoutCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWorkoutHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = await _unitOfWork.Workouts.GetByIdAsync(request.WorkoutId, cancellationToken);

        if (workout == null || workout.UserId != request.UserId)
            return false;

        await _unitOfWork.Workouts.RemoveAsync(workout);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}