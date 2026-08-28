namespace FitTrackApi.Application.Dto.Exercise;

/// <summary>
/// Model for short response of Exercise. For example to display on cards on web page
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Category"></param>
/// <param name="Equipment"></param>
/// <param name="PreviewImage"></param>
public record ExerciseShortResponse(
    Guid Id,
    string Name,
    string Category,
    string? Equipment,
    string? PreviewImage
    );