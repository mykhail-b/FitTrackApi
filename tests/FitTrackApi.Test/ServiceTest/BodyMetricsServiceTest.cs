using FitTrackApi.Application.Dto.User;
using FitTrackApi.Infrastructure.Entity;
using FitTrackApi.Infrastructure.Services;
using FitTrackApi.Test.Configuration;

namespace FitTrackApi.Test.ServiceTest;

public class BodyMetricServiceTest(DatabaseFixture fixture) : IntegrationTestBase(fixture)
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

    private async Task<BodyMetric> CreateTestBodyMetricAsync()
    {
        var user = await CreateTestUserAsync();

        var metric = new BodyMetric
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Height = 180,
            Weight = 75
        };

        DbContext.BodyMetrics.Add(metric);
        await DbContext.SaveChangesAsync();
        return metric;
    }

    [Fact]
    public async Task GetAsync_Should_ReturnBodyMetrics_WhenDataIsValid()
    {
        var metric = await CreateTestBodyMetricAsync();
        var service = new BodyMetricService(DbContext);

        var result = await service.GetAsync(metric.UserId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(180, result.Height);
        Assert.Equal(75, result.Weight);
    }

    [Fact]
    public async Task GetAsync_Should_Throw_WhenBodyMetricsNotFound()
    {
        var service = new BodyMetricService(DbContext);

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.GetAsync("non-existent-id", CancellationToken.None));
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateBodyMetrics_WhenDataIsValid()
    {
        var metric = await CreateTestBodyMetricAsync();
        var service = new BodyMetricService(DbContext);

        var dto = new BodyMetricDto
        {
            Height = 175,
            Weight = 72
        };

        var result = await service.UpdateAsync(metric.UserId, dto, CancellationToken.None);

        Assert.Equal(175, result.Height);
        Assert.Equal(72, result.Weight);

        var updated = DbContext.BodyMetrics.First(b => b.UserId == metric.UserId);
        Assert.Equal(175, updated.Height);
        Assert.Equal(72, updated.Weight);
    }
}