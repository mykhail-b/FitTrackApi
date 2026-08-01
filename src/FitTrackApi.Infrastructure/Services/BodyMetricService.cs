using FitTrackApi.Application.Dto.User;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace FitTrackApi.Infrastructure.Services;

public interface IBodyMetricService
{
    Task<BodyMetricDto> GetAsync(string userId, CancellationToken ct = default);
    Task<BodyMetricDto> UpdateAsync(string userId, BodyMetricDto dto, CancellationToken ct = default);
}

public class BodyMetricService : IBodyMetricService
{
    private readonly DataContext _dbContext;
    public BodyMetricService(DataContext dbContext) => _dbContext = dbContext;

    public async Task<BodyMetricDto> GetAsync(string userId, CancellationToken ct = default)
    {
        var m = await _dbContext.BodyMetrics.AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserId == userId, ct)
            ?? throw new KeyNotFoundException($"Body metrics not found for user {userId}");

        return new BodyMetricDto
        {
            Height = m.Height,
            Weight = m.Weight,
            TargetWeight = m.TargetWeight,
            BirthDate = m.BirthDate,
            Gender = m.Gender,
            ActivityLevel = m.ActivityLevel,
            Goal = m.Goal,
            DailyCalories = m.DailyCalories,
            ProteinGrams = m.ProteinGrams,
            FatGrams = m.FatGrams,
            CarbsGrams = m.CarbsGrams
        };
    }

    public async Task<BodyMetricDto> UpdateAsync(string userId, BodyMetricDto dto, CancellationToken ct = default)
    {
        var m = await _dbContext.BodyMetrics.FirstOrDefaultAsync(b => b.UserId == userId, ct)
            ?? throw new KeyNotFoundException($"UserDetail not found for user {userId}");

        m.Height = dto.Height;
        m.Weight = dto.Weight;
        m.TargetWeight = dto.TargetWeight;
        m.BirthDate = dto.BirthDate;
        m.Gender = dto.Gender;
        m.ActivityLevel = dto.ActivityLevel;
        m.Goal = dto.Goal;
        m.DailyCalories = dto.DailyCalories;
        m.ProteinGrams = dto.ProteinGrams;
        m.FatGrams = dto.FatGrams;
        m.CarbsGrams = dto.CarbsGrams;
        m.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(ct);
        return dto;
    }
}