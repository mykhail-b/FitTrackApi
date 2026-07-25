using FitTrackApi.Server.Cqrs.Handlers.UserHandlers;
using FitTrackApi.Core.Dto.User;
using FitTrackApi.Core.Entity;
using FitTrackApi.Test.Configuration;

namespace FitTrackApi.Test.Cqrs;

public class UserHandlersTest(DatabaseFixture fixture) : IntegrationTestBase(fixture)
{
    private async Task<UserAccount> CreateTestUserAsync()
    {
        var user = new UserAccount
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "test@fittrack.com",
            Email = "test@fittrack.com",
            FullName = "Test User"
        };
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task Handler_Should_UpdateUserContactInfo_WhenCommandIsValid()
    {
        var user = await CreateTestUserAsync();

        var handler = new UpdateUserInfoHandler(DbContext);
        var dto = new UserInfoDto { FullName = "Updated Name", Email = "updated@fittrack.com" };

        var result = await handler.Handle(new UpdateUserInfoCommand(user.Id, dto), CancellationToken.None);

        Assert.True(result);
        var updated = DbContext.Users.First(u => u.Id == user.Id);
        Assert.Equal("Updated Name", updated.FullName);
    }

    [Fact]
    public async Task Handler_Should_ReturnException_WhenUserNotFound()
    {
        var handler = new UpdateUserInfoHandler(DbContext);
        var dto = new UserInfoDto { FullName = "X", Email = "x@x.com" };

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            handler.Handle(new UpdateUserInfoCommand("non-existent-id", dto), CancellationToken.None));
    }

    [Fact]
    public async Task Handler_Should_GetUserBodyMetrics_ById_WhenQueryIsValid()
    {
        var user = await CreateTestUserAsync();

        DbContext.BodyMetrics.Add(new BodyMetric
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Height = 180,
            Weight = 75
        });
        await DbContext.SaveChangesAsync();

        var handler = new GetBodyMetricHandler(DbContext);
        var result = await handler.Handle(new GetBodyMetricQuery(user.Id), CancellationToken.None);

        Assert.Equal(180, result.Height);
    }

    [Fact]
    public async Task Handler_Should_UpdateUserBodyMetrics_WhenCommandIsValid()
    {
        var user = await CreateTestUserAsync();

        DbContext.BodyMetrics.Add(new BodyMetric
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Height = 170,
            Weight = 70
        });
        await DbContext.SaveChangesAsync();

        var handler = new UpdateBodyMetricHandler(DbContext);
        var dto = new BodyMetricDto { Height = 175, Weight = 72 };

        await handler.Handle(new UpdateBodyMetricCommand(user.Id, dto), CancellationToken.None);

        var updated = DbContext.BodyMetrics.First(d => d.UserId == user.Id);
        Assert.Equal(175, updated.Height);
    }
}
