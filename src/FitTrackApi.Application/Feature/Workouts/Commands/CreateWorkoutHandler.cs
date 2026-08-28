using FitTrackApi.Application.Dto.Workout;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using FitTrackApi.Domain.Entity;
using MediatR;

namespace FitTrackApi.Application.Feature.Workouts.Commands;

public record CreateWorkoutCommand(string UserId, CreateWorkoutRequest CreateWorkoutRequest) : IRequest<WorkoutDto>;

internal class CreateWorkoutHandler : IRequestHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkoutMapper _mapper;

    public CreateWorkoutHandler(IUnitOfWork unitOfWork, IWorkoutMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WorkoutDto> Handle(CreateWorkoutCommand request, CancellationToken cancellationToken)
    {
        var workout = new Workout
        {
            UserId = request.UserId,
            Date = request.CreateWorkoutRequest.Date,
            Notes = request.CreateWorkoutRequest.Notes,
            Sets = request.CreateWorkoutRequest.Sets.Select(s => new WorkoutSet
            {
                ExerciseId = s.ExerciseId,
                SetNumber = s.SetNumber,
                Reps = s.Reps,
                Weight = s.Weight
            }).ToList()
        };

        await _unitOfWork.Workouts.AddAsync(workout, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.ToDto(workout);
    }
}