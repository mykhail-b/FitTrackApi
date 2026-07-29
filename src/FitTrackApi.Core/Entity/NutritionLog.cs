namespace FitTrackApi.Core.Entity;

public class NutritionLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateOnly Date { get; set; }

    public int TotalCalories { get; set; }
    public int? TotalProteins { get; set; }
    public int? TotalFats { get; set; }
    public int? TotalCarbs { get; set; }
}
