using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Infrastructure.Entity;
using FitTrackApi.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FitTrackApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // === EF Core ===
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // === Identity ===
        services.AddIdentity<UserAccount, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();

        // === Application service implementations ===
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBodyMetricService, BodyMetricService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IExerciseService, ExerciseService>();

        return services;
    }
}
