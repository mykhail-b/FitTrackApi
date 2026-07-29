namespace FitTrackApi.Core.Entity;

public class BodyMetric
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;
    public UserAccount User { get; set; } = null!;

    // Physical parameters
    public double Height { get; set; }          // height, cm
    public double Weight { get; set; }          // current weight, kg
    public double? TargetWeight { get; set; }    // target weight, kg

    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }

    public ActivityLevel ActivityLevel { get; set; }
    public FitnessGoal Goal { get; set; }

    // Nutritional value and calories (daily intake)
    public double DailyCalories { get; set; }
    public double ProteinGrams { get; set; }
    public double FatGrams { get; set; }
    public double CarbsGrams { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum Gender
{
    Male,
    Female,
    Other
}

public enum ActivityLevel
{
    Sedentary,
    Light,           // (1-3 workouts/week)
    Moderate,        // (3-5 workouts/week)
    Active,          // (6-7 workouts/week)
    VeryActive       
}

public enum FitnessGoal
{
    WeightLoss,
    Maintenance,
    MuscleGain,
    Recomposition
}