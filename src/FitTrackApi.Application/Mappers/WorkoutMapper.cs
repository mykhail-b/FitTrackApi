using FitTrackApi.Application.Dto.Workout;
using FitTrackApi.Domain.Entity;

namespace FitTrackApi.Application.Mappers;

public interface IWorkoutMapper
{
    WorkoutDto ToDto(Workout workout);
    List<WorkoutDto> ToDtoList(IEnumerable<Workout> workouts);
}

public class WorkoutMapper : IWorkoutMapper
{
    public WorkoutDto ToDto(Workout workout)
    {
        return new WorkoutDto(
            workout.Id,
            workout.Date,
            workout.Notes,
            workout.Sets.Select(set => new WorkoutSetDto(
                set.Id,
                set.ExerciseId,
                set.Exercise?.Name ?? string.Empty,
                set.SetNumber,
                set.Reps,
                set.Weight
            )).ToList()
        );
    }

    public List<WorkoutDto> ToDtoList(IEnumerable<Workout> workouts)
    {
        return workouts.Select(ToDto).ToList();
    }
}