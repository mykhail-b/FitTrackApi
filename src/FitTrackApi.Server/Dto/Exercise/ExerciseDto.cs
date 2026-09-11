namespace FitTrackApi.Application.Dto.Exercise;

public record CreateExerciseRequest(
    string Name,
    string? Force,
    string? Mechanic,
    string? Equipment,
    string Category,
    string Muscle,
    string Instruction,
    string Images
    );

public record ExerciseResponse(
    Guid Id,
    string Name,
    string? Force,
    string? Mechanic,
    string? Equipment,
    string Category,
    string Muscle,
    string Instruction,
    string Images
    );

public record ExerciseShortResponse(
    Guid Id,
    string Name,
    string Category,
    string? Equipment,
    string? PreviewImage
    );

public record UpdateExerciseRequest(
    string Name,
    string? Force,
    string? Mechanic,
    string? Equipment,
    string Category,
    string Muscle,
    string Instruction,
    string Images
    );
