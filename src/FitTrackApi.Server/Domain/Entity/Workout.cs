using FitTrackApi.Domain.Entity;
using System.Runtime;

namespace FitTrackApi.Server.Domain.Entity;

public class Workout 
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public List<WorkoutSet> WorkoutSets { get; set; } = new();
}


public class WorkoutSet 
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; }

    public Guid ExerciseId { get; set; }
    public Exercise Exercises { get; set; } = new();

    public int SetNumber { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }
}