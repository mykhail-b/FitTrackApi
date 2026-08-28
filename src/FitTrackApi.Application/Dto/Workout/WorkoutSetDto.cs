namespace FitTrackApi.Application.Dto.Workout;

public record WorkoutSetDto(
    Guid Id,
    Guid ExerciseId,
    string ExerciseName,
    int SetNumber,
    int Reps,
    decimal Weight
    );