using FitTrackApi.Application.Interfaces.RepositoryDI;
using MediatR;

namespace FitTrackApi.Application.Feature.Exercises.Commands;

public record DeleteExerciseCommand(Guid Id) : IRequest<bool>;

internal class DeleteExerciseHandler : IRequestHandler<DeleteExerciseCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExerciseHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
    {
        var exercise = await _unitOfWork.Exercises.GetByIdAsync(request.Id, cancellationToken);
        if (exercise == null)
        {
            throw new KeyNotFoundException($"Food with id {request.Id} not found");
        }
        
        await _unitOfWork.Exercises.RemoveAsync(exercise);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}