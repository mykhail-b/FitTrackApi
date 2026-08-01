using FitTrackApi.Domain.Enums;

namespace FitTrackApi.Infrastructure.Entity;

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