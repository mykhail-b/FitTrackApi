using FitTrackApi.Core.Dto.Workout;
using FitTrackApi.Core.Entity;
using FitTrackApi.Server.Services;
using FitTrackApi.Test.Configuration;

namespace FitTrackApi.Test.ServiceTest;

public class WorkoutServiceTest(DatabaseFixture fixture) : IntegrationTestBase(fixture)
{
    private async Task<UserAccount> CreateTestUserAsync(string id = "test-user-id")
    {
        var user = new UserAccount
        {
            Id = id,
            UserName = "test@fittrack.com",
            Email = "test@fittrack.com",
            FullName = "Test User"
        };

        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task CreateAsync_Should_CreateWorkout_WhenDataIsValid()
    {
        await CreateTestUserAsync();
        var service = new WorkoutService(DbContext);

        var result = await service.CreateAsync(
            "test-user-id",
            DateTime.UtcNow,
            "Leg day",
            new List<WorkoutExerciseDto>(),
            CancellationToken.None);

        Assert.True(result);
        Assert.Single(DbContext.Workouts);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateWorkout_WhenDataIsValid()
    {
        await CreateTestUserAsync();
        var service = new WorkoutService(DbContext);

        await service.CreateAsync(
            "test-user-id",
            DateTime.UtcNow,
            "Original",
            new List<WorkoutExerciseDto>(),
            CancellationToken.None);

        var workout = DbContext.Workouts.First();

        var result = await service.UpdateAsync(
            workout.Id,
            DateTime.UtcNow,
            "Updated",
            new List<WorkoutExerciseDto>(),
            CancellationToken.None);

        Assert.True(result);
        Assert.Equal("Updated", DbContext.Workouts.First().Notes);
    }

    [Fact]
    public async Task RemoveAsync_Should_RemoveWorkout_WhenDataIsValid()
    {
        await CreateTestUserAsync();
        var service = new WorkoutService(DbContext);

        await service.CreateAsync(
            "test-user-id",
            DateTime.UtcNow,
            null,
            new List<WorkoutExerciseDto>(),
            CancellationToken.None);

        var workout = DbContext.Workouts.First();

        var result = await service.RemoveAsync(workout.Id, CancellationToken.None);

        Assert.True(result);
        Assert.Empty(DbContext.Workouts);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnWorkout_WhenWorkoutExists()
    {
        await CreateTestUserAsync();
        var service = new WorkoutService(DbContext);

        await service.CreateAsync(
            "test-user-id",
            DateTime.UtcNow,
            "Push day",
            new List<WorkoutExerciseDto>(),
            CancellationToken.None);

        var workout = DbContext.Workouts.First();

        var result = await service.GetByIdAsync(workout.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Push day", result!.Notes);
    }

    [Fact]
    public async Task GetAllForUserAsync_Should_ReturnAllUserWorkouts_WhenUserHasWorkouts()
    {
        await CreateTestUserAsync();
        var service = new WorkoutService(DbContext);

        await service.CreateAsync(
            "test-user-id",
            DateTime.UtcNow,
            null,
            new List<WorkoutExerciseDto>(),
            CancellationToken.None);

        await service.CreateAsync(
            "test-user-id",
            DateTime.UtcNow,
            null,
            new List<WorkoutExerciseDto>(),
            CancellationToken.None);

        var result = await service.GetAllForUserAsync("test-user-id", CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_WhenWorkoutNotFound()
    {
        var service = new WorkoutService(DbContext);

        var result = await service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }
}