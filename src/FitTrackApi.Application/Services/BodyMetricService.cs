using FitTrackApi.Application.Dto.User;
using FitTrackApi.Application.Interfaces.RepositoryDI;

namespace FitTrackApi.Application.Services;

public interface IBodyMetricService
{
    Task<BodyMetricDto> GetAsync(string userId, CancellationToken ct = default);
    Task<BodyMetricDto> UpdateAsync(string userId, BodyMetricDto dto, CancellationToken ct = default);
}

public class BodyMetricService : IBodyMetricService
{
    private readonly IUnitOfWork _unitOfWork;
    public BodyMetricService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<BodyMetricDto> GetAsync(string userId, CancellationToken ct = default)
    {
        var m = await _unitOfWork.BodyMetrics.GetByUserIdAsync(userId, ct)
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
        var m = await _unitOfWork.BodyMetrics.GetByUserIdAsync(userId, ct)
            ?? throw new KeyNotFoundException($"Body metrics not found for user {userId}");

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

        await _unitOfWork.BodyMetrics.UpdateAsync(m);
        await _unitOfWork.SaveChangesAsync(ct);

        return dto;
    }
}