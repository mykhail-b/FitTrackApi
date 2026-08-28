using FitTrackApi.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FitTrackApi.Infrastructure.IdentityEntity;

public class UserAccount : IdentityUser
{
    public string FullName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
}