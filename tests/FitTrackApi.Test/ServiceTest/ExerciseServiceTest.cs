using FitTrackApi.Infrastructure.Entity;
using FitTrackApi.Infrastructure.Services;
using FitTrackApi.Test.Configuration;

namespace FitTrackApi.Test.ServiceTest;

public class ExerciseServiceTest(DatabaseFixture fixture) : IntegrationTestBase(fixture)
{
    private async Task<Exercise> CreateTestExerciseAsync(string name, int? id = null)
    {
        var exercise = new Exercise
        {
            Name = name,
            Level = "Beginner",
            Category = "Strength",
            Images = new List<string> { $"/images/{name.ToLower().Replace(' ', '-')}.jpg" },
            PrimaryMuscles = new List<string> { "Chest" }
        };

        DbContext.Exercises.Add(exercise);
        await DbContext.SaveChangesAsync();
        return exercise;
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnExerciseDetails_WhenExerciseExists()
    {
        var created = await CreateTestExerciseAsync("Push Up");

        var service = new ExerciseService(DbContext);

        var result = await service.GetByIdAsync(created.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Push Up", result!.Name);
        Assert.Equal("Beginner", result.Level);
        Assert.NotEmpty(result.Images);
        Assert.Contains("/images/push-up.jpg", result.Images);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_WhenExerciseNotFound()
    {
        var service = new ExerciseService(DbContext);

        var result = await service.GetByIdAsync(99999, CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetPagedAsync_Should_ReturnPagedResults()
    {
        // create 15 exercises
        for (var i = 1; i <= 15; i++)
        {
            await CreateTestExerciseAsync($"Exercise {i}");
        }

        var service = new ExerciseService(DbContext);

        var page = await service.GetPagedAsync(2, 10, CancellationToken.None);

        Assert.Equal(15, page.TotalCount);
        Assert.Equal(2, page.PageNumber);
        Assert.Equal(10, page.PageSize);
        Assert.Equal(5, page.Items.Count);
        Assert.Equal("Exercise 11", page.Items.First().Name);
    }
}
