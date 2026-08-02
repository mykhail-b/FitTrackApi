using FitTrackApi.Domain.Enums;

namespace FitTrackApi.Domain.Entity;

public class BodyMetric
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;

    public double Height { get; set; }
    public double Weight { get; set; }
    public double? TargetWeight { get; set; }

    public DateOnly? BirthDate { get; set; }
    public Gender Gender { get; set; }

    public ActivityLevel ActivityLevel { get; set; }
    public FitnessGoal Goal { get; set; }

    public double DailyCalories { get; set; }
    public double ProteinGrams { get; set; }
    public double FatGrams { get; set; }
    public double CarbsGrams { get; set; }

    // Metadata
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}