using FitTrackApi.Application.Dto.User;
using FitTrackApi.Infrastructure.Entity;
using FitTrackApi.Infrastructure.Services;
using FitTrackApi.Test.Configuration;

namespace FitTrackApi.Test.ServiceTest;

public class UserServiceTest(DatabaseFixture fixture) : IntegrationTestBase(fixture)
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
    public async Task GetUserInfoAsync_Should_ReturnUserInfo_WhenUserExists()
    {
        var user = await CreateTestUserAsync();
        var service = new UserService(DbContext);

        var result = await service.GetUserInfoAsync(user.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Test User", result!.FullName);
        Assert.Equal("test@fittrack.com", result.Email);
    }

    [Fact]
    public async Task GetUserInfoAsync_Should_ReturnNull_WhenUserNotFound()
    {
        var service = new UserService(DbContext);

        var result = await service.GetUserInfoAsync("non-existent-id", CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserInfoAsync_Should_UpdateUserInfo_WhenDataIsValid()
    {
        var user = await CreateTestUserAsync();
        var service = new UserService(DbContext);
        var dto = new UserInfoDto
        {
            FullName = "Updated Name",
            Email = "updated@fittrack.com"
        };

        var result = await service.UpdateUserInfoAsync(user.Id, dto, CancellationToken.None);

        Assert.True(result);

        var updated = DbContext.Users.First(u => u.Id == user.Id);
        Assert.Equal("Updated Name", updated.FullName);
        Assert.Equal("updated@fittrack.com", updated.Email);
    }

    [Fact]
    public async Task UpdateUserInfoAsync_Should_ReturnFalse_WhenUserNotFound()
    {
        var service = new UserService(DbContext);
        var dto = new UserInfoDto
        {
            FullName = "X",
            Email = "x@x.com"
        };

        var result = await service.UpdateUserInfoAsync("non-existent-id", dto, CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteUserAsync_Should_DeleteUser_WhenUserExists()
    {
        var user = await CreateTestUserAsync();
        var service = new UserService(DbContext);

        var result = await service.DeleteUserAsync(user.Id, CancellationToken.None);

        Assert.True(result);
        Assert.DoesNotContain(DbContext.Users, u => u.Id == user.Id);
    }

    [Fact]
    public async Task DeleteUserAsync_Should_ReturnFalse_WhenUserNotFound()
    {
        var service = new UserService(DbContext);

        var result = await service.DeleteUserAsync("non-existent-id", CancellationToken.None);

        Assert.False(result);
    }
}