namespace FitTrackApi.Application.Dto.Exercise;

/// <summary>
/// The update request model for Exercise entity
/// </summary>
/// <param name="Name"></param>
/// <param name="Force"></param>
/// <param name="Mechanic"></param>
/// <param name="Equipment"></param>
/// <param name="Category"></param>
/// <param name="MeasurabilityType"></param>
/// <param name="Muscles"></param>
/// <param name="Instructions"></param>
/// <param name="Images"></param>
public record UpdateExerciseRequest(
    string Name,
    string? Force,
    string? Mechanic,
    string? Equipment,
    string Category,
    string? MeasurabilityType,
    List<string> Muscles,
    string Instructions,
    string Images
    );