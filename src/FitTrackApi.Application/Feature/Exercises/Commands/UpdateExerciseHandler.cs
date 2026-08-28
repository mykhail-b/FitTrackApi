using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using MediatR;

namespace FitTrackApi.Application.Feature.Exercises.Commands;

public record UpdateExerciseCommand(Guid Id, UpdateExerciseRequest UpdateExerciseRequest) : IRequest<ExerciseResponse>;

internal class UpdateExerciseHandler : IRequestHandler<UpdateExerciseCommand, ExerciseResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExerciseMapper _mapper;

    public UpdateExerciseHandler(IUnitOfWork unitOfWork, IExerciseMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ExerciseResponse> Handle(UpdateExerciseCommand request, CancellationToken cancellationToken)
    { 
        var exercise = await _unitOfWork.Exercises.GetByIdAsync(request.Id,  cancellationToken);
        
        if (exercise == null)
            throw new KeyNotFoundException($"Exercise {request.Id} not found");
        
        exercise.Name = request.UpdateExerciseRequest.Name;
        exercise.Force = request.UpdateExerciseRequest.Force;
        exercise.Mechanic = request.UpdateExerciseRequest.Mechanic;
        exercise.Equipment = request.UpdateExerciseRequest.Equipment;
        exercise.Category = request.UpdateExerciseRequest.Category;
        exercise.MeasurabilityType = request.UpdateExerciseRequest.MeasurabilityType;
        exercise.Muscles = request.UpdateExerciseRequest.Muscles;
        exercise.Instructions = request.UpdateExerciseRequest.Instructions;
        exercise.Images = request.UpdateExerciseRequest.Images;
        
        await _unitOfWork.Exercises.UpdateAsync(exercise);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.ToResponse(exercise);
    }
}