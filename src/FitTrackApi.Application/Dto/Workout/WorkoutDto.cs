namespace FitTrackApi.Application.Dto.Workout;

public class WorkoutExerciseDto
{
    public int ExerciseId { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }
}

public class WorkoutDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public List<WorkoutExerciseDto> Exercises { get; set; } = new();
}