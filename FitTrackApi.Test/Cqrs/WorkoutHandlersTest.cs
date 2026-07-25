using FitTrackApi.Server.Cqrs.Handlers.WorkoutHandlers;
using FitTrackApi.Core.Dto.Workout;
using FitTrackApi.Test.Configuration;
using FitTrackApi.Core.Entity;

namespace FitTrackApi.Test.Cqrs;

public class WorkoutHandlersTest(DatabaseFixture fixture) : IntegrationTestBase(fixture)
{
    private async Task CreateTestUserAsync(string id = "test-user-id")
    {
        DbContext.Users.Add(new UserAccount
        {
            Id = id,
            UserName = "test@fittrack.com",
            Email = "test@fittrack.com",
            FullName = "Test User"
        });
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_CreateWorkout_WhenCommandIsValid()
    {
        await CreateTestUserAsync();
        var handler = new CreateWorkoutHandler(DbContext);

        var command = new CreateWorkoutCommand("test-user-id", DateTime.UtcNow, "Leg day", new List<WorkoutExerciseDto>());
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result);
        Assert.Single(DbContext.Workouts);
    }

    [Fact]
    public async Task Handler_Should_UpdateWorkout_WhenCommandIsValid()
    {
        await CreateTestUserAsync();
        var createHandler = new CreateWorkoutHandler(DbContext);
        await createHandler.Handle(new CreateWorkoutCommand("test-user-id", DateTime.UtcNow, "Original", new List<WorkoutExerciseDto>()), CancellationToken.None);

        var workout = DbContext.Workouts.First();
        var updateHandler = new UpdateWorkoutHandler(DbContext);

        var result = await updateHandler.Handle(
            new UpdateWorkoutCommand(workout.Id, DateTime.UtcNow, "Updated", new List<WorkoutExerciseDto>()),
            CancellationToken.None);

        Assert.True(result);
        Assert.Equal("Updated", DbContext.Workouts.First().Notes);
    }

    [Fact]
    public async Task Handler_Should_RemoveWorkout_WhenCommandIsValid()
    {
        await CreateTestUserAsync();
        var createHandler = new CreateWorkoutHandler(DbContext);
        await createHandler.Handle(new CreateWorkoutCommand("test-user-id", DateTime.UtcNow, null, new List<WorkoutExerciseDto>()), CancellationToken.None);

        var workout = DbContext.Workouts.First();
        var removeHandler = new RemoveWorkoutHandler(DbContext);

        var result = await removeHandler.Handle(new RemoveWorkoutCommand(workout.Id), CancellationToken.None);

        Assert.True(result);
        Assert.Empty(DbContext.Workouts);
    }

    [Fact]
    public async Task Handler_Should_GetUserWorkout_ById_WhenQueryIsValid()
    {
        await CreateTestUserAsync();
        var createHandler = new CreateWorkoutHandler(DbContext);
        await createHandler.Handle(new CreateWorkoutCommand("test-user-id", DateTime.UtcNow, "Push day", new List<WorkoutExerciseDto>()), CancellationToken.None);

        var workout = DbContext.Workouts.First();
        var getHandler = new GetWorkoutByIdHandler(DbContext);

        var result = await getHandler.Handle(new GetWorkoutByIdQuery(workout.Id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Push day", result!.Notes);
    }

    [Fact]
    public async Task Handler_Should_GetAllUserWorkouts_WhenQueryIsValid()
    {
        await CreateTestUserAsync();
        var createHandler = new CreateWorkoutHandler(DbContext);
        await createHandler.Handle(new CreateWorkoutCommand("test-user-id", DateTime.UtcNow, null, new List<WorkoutExerciseDto>()), CancellationToken.None);
        await createHandler.Handle(new CreateWorkoutCommand("test-user-id", DateTime.UtcNow, null, new List<WorkoutExerciseDto>()), CancellationToken.None);

        var getHandler = new GetAllUserWorkoutsHandler(DbContext);
        var result = await getHandler.Handle(new GetAllUserWorkoutsQuery("test-user-id"), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Handler_Should_ReturnNull_WhenWorkoutNotFound()
    {
        var handler = new GetWorkoutByIdHandler(DbContext);

        var result = await handler.Handle(new GetWorkoutByIdQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.Null(result);
    }
}
