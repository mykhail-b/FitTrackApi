using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Domain.Entity;

namespace FitTrackApi.Application.Mappers;

public interface IExerciseMapper
{
    ExerciseResponse ToResponse(Exercise exercise);
    ExerciseShortResponse ToShortResponse(Exercise exercise);
    List<ExerciseShortResponse> ToShortResponseList(IEnumerable<Exercise> exercises);
}

public class ExerciseMapper : IExerciseMapper
{
    public ExerciseResponse ToResponse(Exercise exercise) => new(
        exercise.Id,
        exercise.Name,
        exercise.Force,
        exercise.Mechanic,
        exercise.Equipment,
        exercise.Category,
        exercise.MeasurabilityType,
        exercise.Muscles,
        exercise.Instructions,
        exercise.Images
    );

    public ExerciseShortResponse ToShortResponse(Exercise exercise) => new(
        exercise.Id,
        exercise.Name,
        exercise.Category,
        exercise.Equipment,
        exercise.Images
    );

    public List<ExerciseShortResponse> ToShortResponseList(IEnumerable<Exercise> exercises) =>
        exercises.Select(ToShortResponse).ToList();
}