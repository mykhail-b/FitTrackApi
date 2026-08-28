namespace FitTrackApi.Application.Interfaces.RepositoryDI;

public interface IUnitOfWork
{
    IExerciseRepository Exercises { get; }
    IWorkoutRepository Workouts { get; }
    IFoodRepository Foods { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}