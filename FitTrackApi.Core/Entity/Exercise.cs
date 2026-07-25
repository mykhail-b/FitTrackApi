namespace FitTrackApi.Core.Entity;

public class Exercise
{
    public int Id { get; set; }          
    public string Name { get; set; } = null!;       // for example: "Alternate_Incline_Dumbbell_Curl"
    public string Force { get; set; } = null!;        // for example: "pull"
    public string Level { get; set; } = null!;        // for example: "beginner"
    public string Mechanic { get; set; } = null!;      // for example: "isolation"
    public string Equipment { get; set; } = null!;     // for example: "dumbbell"

    public List<string> PrimaryMuscles { get; set; } = new();
    public List<string> SecondaryMuscles { get; set; } = new();
    public List<string> Instructions { get; set; } = new();

    public string Category { get; set; } = null!;

    public List<string> Images { get; set; } = new();
}
