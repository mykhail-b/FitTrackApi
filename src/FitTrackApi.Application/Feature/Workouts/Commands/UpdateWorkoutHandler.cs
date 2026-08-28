using FitTrackApi.Application.Dto.Workout;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using FitTrackApi.Domain.Entity;
using MediatR;

namespace FitTrackApi.Application.Feature.Workouts.Commands;

public record UpdateWorkoutCommand(Guid WorkoutId, string UserId, UpdateWorkoutRequest UpdateWorkoutRequest) 
    : IRequest<WorkoutDto>;

internal class UpdateWorkoutHandler : IRequestHandler<UpdateWorkoutCommand, WorkoutDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkoutMapper _mapper;

    public UpdateWorkoutHandler(IUnitOfWork unitOfWork, IWorkoutMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WorkoutDto> Handle(UpdateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = await _unitOfWork.Workouts.GetByIdAsync(request.WorkoutId, cancellationToken);

        if (workout == null || workout.UserId != request.UserId)
            throw new KeyNotFoundException($"Workout {request.WorkoutId} not found");

        workout.Date = request.UpdateWorkoutRequest.Date;
        workout.Notes = request.UpdateWorkoutRequest.Notes;
        workout.Sets = MapSets(request.UpdateWorkoutRequest.Sets);

        await _unitOfWork.Workouts.UpdateAsync(workout);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.ToDto(workout);
    }

    private static List<WorkoutSet> MapSets(List<WorkoutSetDto> sets)
    {
        return sets.Select(s => new WorkoutSet
        {
            ExerciseId = s.ExerciseId,
            SetNumber = s.SetNumber,
            Reps = s.Reps,
            Weight = s.Weight
        }).ToList();
    }
}