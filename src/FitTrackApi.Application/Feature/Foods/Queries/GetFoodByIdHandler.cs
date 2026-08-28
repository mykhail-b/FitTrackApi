using FitTrackApi.Application.Dto.Food;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using MediatR;

namespace FitTrackApi.Application.Feature.Foods.Queries;

public record GetFoodByIdQuery(Guid id, CancellationToken ct = default) : IRequest<FoodDto>;

internal class GetFoodByIdHandler : IRequestHandler<GetFoodByIdQuery, FoodDto>
{
    private readonly IFoodMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetFoodByIdHandler(IFoodMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<FoodDto> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
    {
        var food =  await _unitOfWork.Foods.GetByIdAsync(request.id, request.ct)
                   ?? throw new KeyNotFoundException($"Food {request.id} not found");

        return _mapper.ToDto(food);
    }
}