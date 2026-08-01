using FitTrackApi.Domain.Enums;

namespace FitTrackApi.Application.Dto.User;

public class BodyMetricDto
{
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
}
