using FitTrackApi.Server.Domain.Entity;
using FitTrackApi.Server.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FitTrackApi.Domain.Entity
{
    public class Account 
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public Gender Gender { get; set; }

        public List<Meal> Meals { get; set; } = new();
        public List<Workout> Workouts { get; set; } = new();
    }
}
