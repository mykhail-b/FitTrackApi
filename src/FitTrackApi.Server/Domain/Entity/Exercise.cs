namespace FitTrackApi.Server.Domain.Entity;

public class Exercise 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Force { get; set; }
    public string? Mechanic { get; set; }
    public string? Equipment { get; set; }
    
    public string Category { get; set; } = null!;

    public string Muscle { get; set; }
    public string Instruction { get; set; } 
    public string Images { get; set; } 
}