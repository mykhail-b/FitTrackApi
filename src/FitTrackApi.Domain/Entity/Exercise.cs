namespace FitTrackApi.Domain.Entity;

public class Exercise : EntityBase
{
    public string Name { get; set; } = null!;
    public string? Force { get; set; }
    public string? Mechanic { get; set; }
    public string? Equipment { get; set; }
    
    public string Category { get; set; } = null!;

    public string? MeasurabilityType { get; set; }
    public List<string> Muscles { get; set; }
    public string Instructions { get; set; } 
    public string Images { get; set; } 
}