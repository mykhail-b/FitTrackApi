namespace FitTrackApi.Application.Dto.Workout;

/// <summary>
/// The cretion request of Wokrout from client
/// </summary>
/// <param name="Date"></param>
/// <param name="Notes"></param>
/// <param name="Sets"></param>
public record CreateWorkoutRequest(
    DateTime Date,
    string? Notes,
    List<WorkoutSetDto> Sets
);