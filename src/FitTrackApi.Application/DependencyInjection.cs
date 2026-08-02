using FitTrackApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FitTrackApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBodyMetricService, BodyMetricService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IExerciseService, ExerciseService>();

        return services;
    }
}
