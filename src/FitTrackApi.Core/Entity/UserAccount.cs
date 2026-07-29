using Microsoft.AspNetCore.Identity;

namespace FitTrackApi.Core.Entity;

public class UserAccount : IdentityUser
{
    public string FullName { get; set; } = null!;
}