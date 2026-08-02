using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Application.Interfaces.RepositoryDI;

namespace FitTrackApi.Application.Services;

public interface IExerciseService
{
    Task<ExerciseDetailsResult?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedListResult<ExerciseListItem>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}

public class ExerciseService : IExerciseService
{
    private readonly IUnitOfWork _unitOfWork;
    public ExerciseService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<ExerciseDetailsResult?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var e = await _unitOfWork.Exercises.GetByIdAsync(id, ct);
        if (e is null) return null;

        return new ExerciseDetailsResult
        {
            Id = e.Id,
            Name = e.Name,
            Force = e.Force,
            Level = e.Level,
            Mechanic = e.Mechanic,
            Equipment = e.Equipment,
            PrimaryMuscles = e.PrimaryMuscles,
            SecondaryMuscles = e.SecondaryMuscles,
            Instructions = e.Instructions,
            Category = e.Category,
            Images = e.Images
        };
    }

    public async Task<PagedListResult<ExerciseListItem>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var totalCount = await _unitOfWork.Exercises.CountAsync(ct);
        var items = await _unitOfWork.Exercises.GetPagedAsync(pageNumber, pageSize, ct);

        return new PagedListResult<ExerciseListItem>
        {
            Items = items.Select(e => new ExerciseListItem
            {
                Id = e.Id,
                Name = e.Name,
                Image = e.Images.FirstOrDefault() ?? string.Empty
            }).ToList(),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}