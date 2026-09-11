using FitTrackApi.Server.Services.Infrastructure;
using FitTrackApi.Server.Services.Public;

namespace FitTrackApi.Server.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddServicesSetup(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IMealService, MealService>();
            services.AddScoped<IWorkoutService, WorkoutService>();
            services.AddScoped<IExerciseService, IExerciseService>();

            return services;
        }
    }
}
