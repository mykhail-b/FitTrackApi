using FitTrackApi.Infrastructure.Data;

namespace FitTrackApi.Server.Services.Public
{
    public interface IMealService
    {
        Task AddMealToProfile();
        Task CreateMeal();
        Task UpdateMealById();
        Task DeleteMealById();
    }
    public class MealService : IMealService
    {
        private readonly DataContext _context;

        public MealService(DataContext context)
        {
            _context = context;
        }

        public async Task AddMealToProfile()
        {
        }
        public async Task CreateMeal() 
        { 
        
        }
        public async Task UpdateMealById() 
        {
        
        }
        public async Task DeleteMealById() 
        {
        
        }
    }
}
