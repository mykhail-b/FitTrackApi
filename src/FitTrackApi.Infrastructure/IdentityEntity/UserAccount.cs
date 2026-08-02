using Microsoft.AspNetCore.Identity;

namespace FitTrackApi.Infrastructure.IdentityEntity;

public class UserAccount : IdentityUser
{
    public string FullName { get; set; } = null!;
}