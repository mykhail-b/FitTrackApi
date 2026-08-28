using FitTrackApi.Application.Dto.Food;
using FitTrackApi.Domain.Entity;

namespace FitTrackApi.Application.Mappers;

public interface IFoodMapper
{
    FoodDto ToDto(Food food);
    List<FoodDto> ToDtoList(IEnumerable<Food> data);
}

public class FoodMapper : IFoodMapper
{
    public FoodDto ToDto(Food food) => new(
        food.Id,
        food.Name,
        food.Calories,
        food.Protein,
        food.Fat,
        food.Carbs
    );

    public List<FoodDto> ToDtoList(IEnumerable<Food> data) =>
        data.Select(ToDto).ToList();
}