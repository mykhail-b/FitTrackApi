namespace FitTrackApi.Application.Dto.Workout;

public record WorkoutDto(
    Guid Id,
    DateTime Date,
    string? Notes,
    List<WorkoutSetDto> Sets
    );

public record WorkoutSetDto(
    Guid Id,
    Guid ExerciseId,
    string ExerciseName,
    int SetNumber,
    int Reps,
    decimal Weight
    );

public record UpdateWorkoutRequest(
    DateTime Date,
    string? Notes,
    List<WorkoutSetDto> Sets
    );

public record CreateWorkoutRequest(
    DateTime Date,
    string? Notes,
    List<WorkoutSetDto> Sets
);