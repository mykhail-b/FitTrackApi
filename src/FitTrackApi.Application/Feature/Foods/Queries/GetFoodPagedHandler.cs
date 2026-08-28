using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Food;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using MediatR;

namespace FitTrackApi.Application.Feature.Foods.Queries;

public record GetFoodPagedQuery(int PageNumber = 1, int PageSize = 10) 
    : IRequest<PagedListResponse<FoodDto>>;

internal class GetFoodPagedHandler : IRequestHandler<GetFoodPagedQuery, PagedListResponse<FoodDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFoodMapper _mapper;

    public GetFoodPagedHandler(IUnitOfWork unitOfWork, IFoodMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedListResponse<FoodDto>> Handle(GetFoodPagedQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var totalCount = await _unitOfWork.Foods.CountAsync(cancellationToken);
        var items = await _unitOfWork.Foods.GetPagedAsync(pageNumber, pageSize, cancellationToken);

        return new PagedListResponse<FoodDto>
        {
            Items = _mapper.ToDtoList(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}