namespace FitTrackApi.Domain.Entity;

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Force { get; set; }
    public string? Mechanic { get; set; }
    public string? Equipment { get; set; }

    public List<string> PrimaryMuscles { get; set; } = new();
    public List<string> SecondaryMuscles { get; set; } = new();
    public List<string> Instructions { get; set; } = new();

    public string Category { get; set; } = null!;

    public List<string> Images { get; set; } = new();

    public string MeasurabilityType { get; set; } = "WeightAndReps";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}