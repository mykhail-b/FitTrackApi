namespace FitTrackApi.Application.Dto.Food;

/// <summary>
/// DTO for food transfer to client
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Calories"></param>
/// <param name="Protein"></param>
/// <param name="Fat"></param>
/// <param name="Carbs"></param>
public record FoodDto(
    Guid Id,
    string Name,
    double Calories,
    double Protein,
    double Fat,
    double Carbs
);