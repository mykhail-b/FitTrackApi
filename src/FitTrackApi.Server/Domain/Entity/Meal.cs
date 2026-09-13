namespace FitTrackApi.Domain.Entity
{
    public class Meal 
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public DateTime Date { get; set; }

        public double TotalCalories { get; set; }
        public double TotalProtein { get; set; }
        public double TotalFat { get; set; }
        public double TotalCarbs { get; set; }
    }
}