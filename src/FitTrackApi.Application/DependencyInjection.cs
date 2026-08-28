using System.Reflection;
using FitTrackApi.Application.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace FitTrackApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Mappers
        services.AddScoped<IWorkoutMapper, WorkoutMapper>();
        services.AddScoped<IExerciseMapper, ExerciseMapper>();
        services.AddScoped<IFoodMapper, FoodMapper>();
        
        //MediatR
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        
        return services;
    }
}
