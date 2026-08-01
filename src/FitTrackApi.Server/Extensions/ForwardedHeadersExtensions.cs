using Microsoft.AspNetCore.HttpOverrides;

namespace FitTrackApi.Server.Extensions;

public static class ForwardedHeadersExtensions
{
    public static IServiceCollection AddForwardedHeadersSetup(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto |
                ForwardedHeaders.XForwardedHost;
        });

        return services;
    }
}