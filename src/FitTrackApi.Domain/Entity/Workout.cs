namespace FitTrackApi.Domain.Entity;

public class Workout : EntityBase
{
    public required string UserId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    public List<WorkoutSet> Sets { get; set; } = new();
}


public class WorkoutSet : EntityBase
{
    public Guid WorkoutId { get; set; }
    public Workout? Workout { get; set; }

    public Guid ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }

    public int SetNumber { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }
}