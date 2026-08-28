using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Application.Mappers;
using MediatR;

namespace FitTrackApi.Application.Feature.Exercises.Queries;

public record GetExerciseByIdQuery(Guid Id) : IRequest<ExerciseResponse>;

internal class GetExerciseByIdHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExerciseMapper _mapper;

    public GetExerciseByIdHandler(IUnitOfWork unitOfWork, IExerciseMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ExerciseResponse> Handle(GetExerciseByIdQuery request, CancellationToken cancellationToken)
    {
        var exercise = await _unitOfWork.Exercises.GetByIdAsync(request.Id, cancellationToken);
        
        if  (exercise == null)
            throw new KeyNotFoundException("");
        
        return _mapper.ToResponse(exercise);
    }
}