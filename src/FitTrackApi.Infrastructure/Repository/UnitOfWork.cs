using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Infrastructure.Data;
namespace FitTrackApi.Infrastructure.Repository;

/// <summary>
/// 
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public IExerciseRepository Exercises { get; }
    public IWorkoutRepository Workouts { get; }
    public IFoodRepository Foods { get; }

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Exercises = new ExerciseRepository(context);
        Workouts = new WorkoutRepository(context);
        Foods = new FoodRepository(context);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}