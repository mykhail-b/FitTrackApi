namespace FitTrackApi.Core.Dto.Exercise;

public class ExerciseDetailsResult
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Force { get; set; } = null!;
    public string Level { get; set; } = null!;
    public string Mechanic { get; set; } = null!;
    public string Equipment { get; set; } = null!;
    public List<string> PrimaryMuscles { get; set; } = new();
    public List<string> SecondaryMuscles { get; set; } = new();
    public List<string> Instructions { get; set; } = new();
    public string Category { get; set; } = null!;
    public List<string> Images { get; set; } = new();
}
