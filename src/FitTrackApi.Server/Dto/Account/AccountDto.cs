using FitTrackApi.Server.Domain.Enums;

namespace FitTrackApi.Server.Dto.User;

public record AccountDto
(
    string FullName,
    string Email,
    
    DateOnly BirthDate,
    Gender Gender
);