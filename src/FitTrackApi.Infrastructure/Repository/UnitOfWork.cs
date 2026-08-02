using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Infrastructure.Data;
namespace FitTrackApi.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public IExerciseRepository Exercises { get; }
    public IWorkoutRepository Workouts { get; }
    public IBodyMetricRepository BodyMetrics { get; }

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Exercises = new ExerciseRepository(context);
        Workouts = new WorkoutRepository(context);
        BodyMetrics = new BodyMetricRepository(context);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}