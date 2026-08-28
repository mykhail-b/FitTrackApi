using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using MediatR;

namespace FitTrackApi.Application.Feature.Exercises.Queries;

public record GetExercisePagedQuery(int PageNumber, int PageSize) : IRequest<PagedListResponse<ExerciseShortResponse>>;

internal class GetExercisePagedHandler : IRequestHandler<GetExercisePagedQuery, PagedListResponse<ExerciseShortResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExerciseMapper _mapper;

    public GetExercisePagedHandler(IUnitOfWork unitOfWork, IExerciseMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedListResponse<ExerciseShortResponse>> Handle(GetExercisePagedQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

        var totalCount = await _unitOfWork.Exercises.CountAsync(cancellationToken);
        var items = await _unitOfWork.Exercises.GetAllAsync(pageNumber, pageSize, cancellationToken);

        return new PagedListResponse<ExerciseShortResponse>
        {
            Items = _mapper.ToShortResponseList(items),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}