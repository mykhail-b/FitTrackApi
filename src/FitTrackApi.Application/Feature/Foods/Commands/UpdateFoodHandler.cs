using FitTrackApi.Application.Dto.Food;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using MediatR;

namespace FitTrackApi.Application.Feature.Foods.Commands;

public record UpdateFoodCommand(Guid Id, UpdateFoodRequest UpdateFoodRequest) : IRequest<FoodDto>;

internal class UpdateFoodHandler : IRequestHandler<UpdateFoodCommand, FoodDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodMapper _mapper;

    public UpdateFoodHandler(IUnitOfWork unitOfWork, IFoodMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper =  mapper;
    }

    public async Task<FoodDto> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var food = await _unitOfWork.Foods.GetByIdAsync(request.Id, cancellationToken);
        
        if (food == null)
            throw new KeyNotFoundException($"Food {request.Id} not found");
        
        food.Name = request.UpdateFoodRequest.Name;
        food.Calories = request.UpdateFoodRequest.Calories;
        food.Protein = request.UpdateFoodRequest.Protein;
        food.Fat = request.UpdateFoodRequest.Fat;
        food.Carbs = request.UpdateFoodRequest.Carbs;
        
        await _unitOfWork.Foods.UpdateAsync(food);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.ToDto(food);
    }
}