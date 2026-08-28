using FitTrackApi.Application.Dto.Food;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using FitTrackApi.Domain.Entity;
using MediatR;

namespace FitTrackApi.Application.Feature.Foods.Commands;

public record CreateFoodCommand(CreateFoodRequest CreateFoodRequest) : IRequest<FoodDto>;

internal class CreateFoodHandler : IRequestHandler<CreateFoodCommand, FoodDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodMapper _mapper;

    public CreateFoodHandler(IUnitOfWork unitOfWork, IFoodMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FoodDto> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        var food = new Food
        {
            Name = request.CreateFoodRequest.Name,
            Calories = request.CreateFoodRequest.Calories,
            Carbs = request.CreateFoodRequest.Carbs,
            Fat = request.CreateFoodRequest.Fat,
            Protein = request.CreateFoodRequest.Protein
        };
        
        await _unitOfWork.Foods.AddAsync(food, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return _mapper.ToDto(food);
    }
}