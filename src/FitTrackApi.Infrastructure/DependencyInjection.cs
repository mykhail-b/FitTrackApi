using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Infrastructure.IdentityEntity;
using FitTrackApi.Infrastructure.Repository;
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

        // === Infrastructure service implementations ===
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();


        // Repositories and UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IExerciseRepository, ExerciseRepository>();
        services.AddScoped<IWorkoutRepository, WorkoutRepository>();
        services.AddScoped<IBodyMetricRepository, BodyMetricRepository>();

        return services;
    }
}
