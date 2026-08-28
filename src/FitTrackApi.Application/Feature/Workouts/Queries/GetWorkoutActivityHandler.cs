using FitTrackApi.Application.Interfaces.RepositoryDI;
using MediatR;

namespace FitTrackApi.Application.Feature.Workouts.Queries;

public record GetWorkoutActivityQuery(string UserId) : IRequest<List<DateOnly>>;

internal class GetWorkoutActivityHandler : IRequestHandler<GetWorkoutActivityQuery, List<DateOnly>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWorkoutActivityHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<List<DateOnly>> Handle(GetWorkoutActivityQuery request, CancellationToken cancellationToken)
    {
        return _unitOfWork.Workouts.GetWorkoutActivityAsync(request.UserId, cancellationToken);
    }
}