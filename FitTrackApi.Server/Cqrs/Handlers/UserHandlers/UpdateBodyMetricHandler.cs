using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using FitTrackApi.Core.Dto.User;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.UserHandlers;

public record UpdateBodyMetricCommand(string UserId, BodyMetricDto BodyMetric);

public class UpdateBodyMetricHandler : ICommandHandler<UpdateBodyMetricCommand, BodyMetricDto>
{
    private readonly DataContext _dbContext;

    public UpdateBodyMetricHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BodyMetricDto> Handle(UpdateBodyMetricCommand command, CancellationToken cancellationToken = default)
    {
        var details = await _dbContext.BodyMetrics
            .FirstOrDefaultAsync(d => d.UserId == command.UserId, cancellationToken);

        if (details is null)
            throw new KeyNotFoundException($"UserDetail not found for user {command.UserId}");

        var info = command.BodyMetric;

        details.Height = info.Height;
        details.Weight = info.Weight;
        details.TargetWeight = info.TargetWeight;
        details.BirthDate = info.BirthDate;
        details.Gender = info.Gender;
        details.ActivityLevel = info.ActivityLevel;
        details.Goal = info.Goal;
        details.DailyCalories = info.DailyCalories;
        details.ProteinGrams = info.ProteinGrams;
        details.FatGrams = info.FatGrams;
        details.CarbsGrams = info.CarbsGrams;
        details.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return info;
    }
}