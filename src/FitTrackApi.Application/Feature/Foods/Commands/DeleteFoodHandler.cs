using FitTrackApi.Application.Interfaces.RepositoryDI;
using MediatR;

namespace FitTrackApi.Application.Feature.Foods.Commands;

public record DeleteFoodCommand(Guid Id) : IRequest<bool>;

internal class DeleteFoodHandler : IRequestHandler<DeleteFoodCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFoodHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        var food = await _unitOfWork.Foods.GetByIdAsync(request.Id, cancellationToken);
        if (food == null)
        {
            throw new KeyNotFoundException($"Food with id {request.Id} not found");
        }
        
        await _unitOfWork.Foods.RemoveAsync(food);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}