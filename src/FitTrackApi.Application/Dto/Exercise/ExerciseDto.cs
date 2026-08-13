namespace FitTrackApi.Application.Dto.Exercise;

using System.Text.Json.Serialization;

public class ExerciseDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("equipment")]
    public string? Equipment { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; } = null!;

    [JsonPropertyName("secondary_muscles")]
    public List<string> SecondaryMuscles { get; set; } = new();

    [JsonPropertyName("instructions")]
    public Dictionary<string, string> Instructions { get; set; } = new();

    [JsonPropertyName("image")]
    public string Image { get; set; } = null!;
}