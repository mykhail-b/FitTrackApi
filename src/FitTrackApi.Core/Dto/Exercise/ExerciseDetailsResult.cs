namespace FitTrackApi.Core.Dto.Exercise;

public class ExerciseDetailsResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Force { get; set; }
    public string Level { get; set; } = string.Empty;
    public string? Mechanic { get; set; }
    public string? Equipment { get; set; }

    public List<string> PrimaryMuscles { get; set; } = new();
    public List<string> SecondaryMuscles { get; set; } = new();
    public List<string> Instructions { get; set; } = new();

    public string Category { get; set; } = string.Empty;

    public List<string> Images { get; set; } = new();

    public string MeasurabilityType { get; set; } = string.Empty;
    public bool IsSystem { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}