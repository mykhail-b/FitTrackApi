using FitTrackApi.Core.Dto.Workout;
using FitTrackApi.Core.Entity;

namespace FitTrackApi.Core;

public static class WorkoutExerciseFactory
{
    public static List<WorkoutExercise> BuildFrom(Guid workoutId, Workout workout, List<WorkoutExerciseDto> dtos)
    {
        return dtos.Select(ex => new WorkoutExercise
        {
            Id = Guid.NewGuid(),
            WorkoutId = workoutId,
            Workout = workout,
            ExerciseId = ex.ExerciseId,
            Sets = ex.Sets,
            Reps = ex.Reps,
            Weight = ex.Weight
        }).ToList();
    }
}
