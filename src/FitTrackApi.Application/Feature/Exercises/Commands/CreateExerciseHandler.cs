using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using FitTrackApi.Domain.Entity;
using MediatR;

namespace FitTrackApi.Application.Feature.Exercises.Commands;

public record CreateExerciseCommand(CreateExerciseRequest CreateExerciseRequest) : IRequest<ExerciseResponse>;

internal class CreateExerciseHandler : IRequestHandler<CreateExerciseCommand, ExerciseResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExerciseMapper _mapper;

    public CreateExerciseHandler(IUnitOfWork unitOfWork, IExerciseMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ExerciseResponse> Handle(CreateExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = new Exercise
        {
            Name = request.CreateExerciseRequest.Name,
            Force = request.CreateExerciseRequest.Force,
            Mechanic = request.CreateExerciseRequest.Mechanic,
            Equipment = request.CreateExerciseRequest.Equipment,
            Category =  request.CreateExerciseRequest.Category,
            MeasurabilityType =  request.CreateExerciseRequest.MeasurabilityType,
            Muscles =  request.CreateExerciseRequest.Muscles,
            Instructions =  request.CreateExerciseRequest.Instructions,
            Images = request.CreateExerciseRequest.Images,
        };
        
        await _unitOfWork.Exercises.AddAsync(exercise, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.ToResponse(exercise);
    }
}