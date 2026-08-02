namespace FitTrackApi.Domain.Entity;

public class Workout
{
    public Guid Id { get; set; }
    public required string UserId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public ICollection<WorkoutExercise> Exercises { get; set; } = new List<WorkoutExercise>();
}

public class WorkoutExercise
{
    public Guid Id { get; set; }

    public Guid WorkoutId { get; set; }
    public Workout? Workout { get; set; }

    public int ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }

    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }
}