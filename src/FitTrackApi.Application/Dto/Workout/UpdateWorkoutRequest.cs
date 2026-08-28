namespace FitTrackApi.Application.Dto.Workout;

/// <summary>
/// 
/// </summary>
/// <param name="Date"></param>
/// <param name="Notes"></param>
/// <param name="Sets"></param>
public record UpdateWorkoutRequest(
    DateTime Date,
    string? Notes,
    List<WorkoutSetDto> Sets
    );