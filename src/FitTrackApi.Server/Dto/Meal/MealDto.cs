

namespace FitTrackApi.Application.Dto.Meal;

public record CreateMealDto(
    DateOnly Date,
    List<CreateMealItemDto> Items);

public record CreateMealItemDto(
    Guid FoodId,
    double QuantityGrams);

public record MealDto(
    Guid Id,
    DateOnly Date,
    double TotalCalories,
    double TotalProtein,
    double TotalFat,
    double TotalCarbs,
    List<MealItemDto> Items);

public record MealItemDto(
    Guid FoodId,
    string FoodName,
    double QuantityGrams,
    double Calories,
    double Protein,
    double Fat,
    double Carbs);