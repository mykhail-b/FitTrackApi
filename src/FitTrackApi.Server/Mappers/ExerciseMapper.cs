using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Server.Domain.Entity;

namespace FitTrackApi.Server.Mappers;

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
        exercise.Muscle,
        exercise.Instruction,
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