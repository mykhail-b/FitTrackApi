namespace FitTrackApi.Application.Dto.ApiResponses;

public sealed record UserResponse(
    string Id,
    string? Username,
    string FullName
);