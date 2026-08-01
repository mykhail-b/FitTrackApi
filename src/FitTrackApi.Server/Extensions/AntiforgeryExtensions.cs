namespace FitTrackApi.Server.Extensions;

public static class AntiforgeryExtensions
{
    public static IServiceCollection AddAntiforgerySetup(this IServiceCollection services)
    {
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-XSRF-TOKEN";
            options.Cookie.Name = "XSRF-TOKEN";
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = false;
        });

        return services;
    }
}
