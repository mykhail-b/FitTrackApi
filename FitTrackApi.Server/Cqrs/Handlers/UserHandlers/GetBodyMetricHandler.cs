using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using FitTrackApi.Core.Dto.User;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.UserHandlers;

public record GetBodyMetricQuery(string UserId);

public class GetBodyMetricHandler : IQueryHandler<GetBodyMetricQuery, BodyMetricDto>
{
    private readonly DataContext _dbContext;

    public GetBodyMetricHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BodyMetricDto> Handle(GetBodyMetricQuery query, CancellationToken cancellationToken = default)
    {
        var bodyMetric = await _dbContext.BodyMetrics
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.UserId == query.UserId, cancellationToken);

        if (bodyMetric is null)
            throw new KeyNotFoundException($"Body metrics not found for user {query.UserId}");

        return new BodyMetricDto
        {
            Height = bodyMetric.Height,
            Weight = bodyMetric.Weight,
            TargetWeight = bodyMetric.TargetWeight,
            BirthDate = bodyMetric.BirthDate,
            Gender = bodyMetric.Gender,
            ActivityLevel = bodyMetric.ActivityLevel,
            Goal = bodyMetric.Goal,
            DailyCalories = bodyMetric.DailyCalories,
            ProteinGrams = bodyMetric.ProteinGrams,
            FatGrams = bodyMetric.FatGrams,
            CarbsGrams = bodyMetric.CarbsGrams
        };
    }
}