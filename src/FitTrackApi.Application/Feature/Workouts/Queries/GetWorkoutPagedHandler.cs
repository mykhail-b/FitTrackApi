using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Workout;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using MediatR;

namespace FitTrackApi.Application.Feature.Workouts.Queries;

public record GetWorkoutPagedQuery(string UserId, int PageNumber = 1, int PageSize = 10) 
    : IRequest<PagedListResponse<WorkoutDto>>;

internal class GetWorkoutPagedHandler : IRequestHandler<GetWorkoutPagedQuery, PagedListResponse<WorkoutDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkoutMapper _mapper;

    public GetWorkoutPagedHandler(IUnitOfWork unitOfWork, IWorkoutMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedListResponse<WorkoutDto>> Handle(GetWorkoutPagedQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var totalCount = await _unitOfWork.Workouts.CountAsync(request.UserId, cancellationToken);
        var items = await _unitOfWork.Workouts.GetPagedAsync(request.UserId, pageNumber, pageSize, cancellationToken);

        return new PagedListResponse<WorkoutDto>
        {
            Items = _mapper.ToDtoList(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}