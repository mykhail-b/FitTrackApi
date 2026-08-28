namespace FitTrackApi.Application.Dto.Food;

/// <summary>
/// Data model for transfer the create request of Food
/// </summary>
/// <param name="Name"></param>
/// <param name="Calories"></param>
/// <param name="Protein"></param>
/// <param name="Fat"></param>
/// <param name="Carbs"></param>
public record CreateFoodRequest(
    string Name,
    double Calories,
    double Protein,
    double Fat,
    double Carbs
    );