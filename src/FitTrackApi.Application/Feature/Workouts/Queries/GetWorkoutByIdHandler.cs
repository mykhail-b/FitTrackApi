using FitTrackApi.Application.Dto.Workout;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using MediatR;

namespace FitTrackApi.Application.Feature.Workouts.Queries;

public record GetWorkoutByIdQuery(Guid WorkoutId, string UserId) : IRequest<WorkoutDto>;

internal class GetWorkoutByIdHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkoutMapper _mapper;

    public GetWorkoutByIdHandler(IUnitOfWork unitOfWork, IWorkoutMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WorkoutDto> Handle(GetWorkoutByIdQuery request, CancellationToken cancellationToken)
    {
        var workout = await _unitOfWork.Workouts.GetByIdAsync(request.WorkoutId, cancellationToken);

        if (workout == null || workout.UserId != request.UserId)
            throw new KeyNotFoundException($"Workout with id {request.WorkoutId} not found");

        return _mapper.ToDto(workout);
    }
}