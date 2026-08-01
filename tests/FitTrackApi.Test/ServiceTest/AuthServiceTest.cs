using FitTrackApi.Application.Dto;
using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Infrastructure.Entity;
using FitTrackApi.Infrastructure.Services;
using FitTrackApi.Test.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace FitTrackApi.Test.ServiceTest;

public class AuthServiceTest(DatabaseFixture fixture) : IntegrationTestBase(fixture)
{
    private (UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager) CreateManagers(DataContext dbContext)
    {
        var services = new ServiceCollection();
        services.AddSingleton(dbContext);
        services.AddHttpContextAccessor();
        services.AddLogging();
        services.AddIdentity<UserAccount, IdentityRole>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        var provider = services.BuildServiceProvider();
        return (provider.GetRequiredService<UserManager<UserAccount>>(),
                provider.GetRequiredService<SignInManager<UserAccount>>());
    }

    [Fact]
    public async Task RegisterAsync_Should_CreateUser_WhenDataIsValid()
    {
        var (userManager, signInManager) = CreateManagers(DbContext);
        var emailServiceMock = new Mock<IEmailService>();

        var authService = new AuthService(userManager, signInManager, emailServiceMock.Object);

        var request = new RegisterRequest
        {
            Email = "newuser@fittrack.com",
            Password = "Test1234!",
            FullName = "New User"
        };

        var result = await authService.RegisterAsync(request);

        Assert.True(result.Succeeded);
        emailServiceMock.Verify(e => e.SendEmail(
            request.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_Should_Fail_WhenUserAlreadyExists()
    {
        var (userManager, signInManager) = CreateManagers(DbContext);
        var emailServiceMock = new Mock<IEmailService>();
        var authService = new AuthService(userManager, signInManager, emailServiceMock.Object);

        var request = new RegisterRequest { Email = "dup@fittrack.com", Password = "Test1234!", FullName = "Dup User" };

        await authService.RegisterAsync(request);
        var secondResult = await authService.RegisterAsync(request);

        Assert.False(secondResult.Succeeded);
    }
}
