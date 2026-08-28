using FitTrackApi.Domain.Enums;

namespace FitTrackApi.Application.Dto.User;

/// <summary>
/// Represents a universal DTO for reading and updating data by the user
/// </summary>
public record UserDto
(
    string FullName,
    string Email,
    
    DateOnly BirthDate,
    Gender Gender
);