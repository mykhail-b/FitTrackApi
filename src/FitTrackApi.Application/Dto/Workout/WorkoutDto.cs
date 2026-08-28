namespace FitTrackApi.Application.Dto.Workout;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="UserId"></param>
/// <param name="Date"></param>
/// <param name="Notes"></param>
/// <param name="Sets"></param>
public record WorkoutDto(
    Guid Id,
    DateTime Date,
    string? Notes,
    List<WorkoutSetDto> Sets
    );