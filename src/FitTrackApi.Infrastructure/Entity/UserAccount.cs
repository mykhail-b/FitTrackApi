using Microsoft.AspNetCore.Identity;

namespace FitTrackApi.Infrastructure.Entity;

public class UserAccount : IdentityUser
{
    public string FullName { get; set; } = null!;
}