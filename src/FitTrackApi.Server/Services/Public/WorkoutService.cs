using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Workout;
using FitTrackApi.Domain.Entity;
using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Server.Domain.Entity;
using FitTrackApi.Server.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FitTrackApi.Server.Services.Public
{
    public interface IWorkoutService
    {
        Task<WorkoutDto> CreateAsync(Guid accountId, CreateWorkoutRequest request, CancellationToken cancellationToken);
        Task<WorkoutDto> UpdateAsync(Guid workoutId, Guid accountId, UpdateWorkoutRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid workoutId, Guid accountId, CancellationToken cancellationToken);
        Task<WorkoutDto> GetByIdAsync(Guid workoutId, Guid accountId, CancellationToken cancellationToken);
        Task<PagedListResponse<WorkoutDto>> GetPagedAsync(Guid accountId, int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<List<DateOnly>> GetActivityAsync(Guid accountId, CancellationToken cancellationToken);
    }
    internal class WorkoutService : IWorkoutService
    {
        private readonly DataContext _context;
        private readonly IWorkoutMapper _mapper;

        public WorkoutService(DataContext context, IWorkoutMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WorkoutDto> CreateAsync(Guid accountId, CreateWorkoutRequest request, CancellationToken cancellationToken)
        {
            var workout = new Workout
            {
                AccountId = accountId,
                Date = request.Date,
                Notes = request.Notes,
                WorkoutSets = request.Sets.Select(s => new WorkoutSet
                {
                    ExerciseId = s.ExerciseId,
                    SetNumber = s.SetNumber,
                    Reps = s.Reps,
                    Weight = s.Weight
                }).ToList()
            };

            await _context.Workouts.AddAsync(workout, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.ToDto(workout);
        }

        public async Task<WorkoutDto> UpdateAsync(Guid workoutId, Guid accountId, UpdateWorkoutRequest request, CancellationToken cancellationToken)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutSets)
                .FirstOrDefaultAsync(w => w.Id == workoutId, cancellationToken);

            if (workout == null || workout.AccountId != accountId)
                throw new KeyNotFoundException($"Workout {workoutId} not found");

            workout.Date = request.Date;
            workout.Notes = request.Notes;

            _context.RemoveRange(workout.WorkoutSets);
            workout.WorkoutSets = MapSets(request.Sets);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.ToDto(workout);
        }

        public async Task<bool> DeleteAsync(Guid workoutId, Guid accountId, CancellationToken cancellationToken)
        {
            var workout = await _context.Workouts.FirstOrDefaultAsync(w => w.Id == workoutId, cancellationToken);

            if (workout == null || workout.AccountId != accountId)
                return false;

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<WorkoutDto> GetByIdAsync(Guid workoutId, Guid accountId, CancellationToken cancellationToken)
        {
            var workout = await _context.Workouts
                .AsNoTracking()
                .Include(w => w.WorkoutSets)
                .FirstOrDefaultAsync(w => w.Id == workoutId, cancellationToken);

            if (workout == null || workout.AccountId != accountId)
                throw new KeyNotFoundException($"Workout with id {workoutId} not found");

            return _mapper.ToDto(workout);
        }

        public async Task<PagedListResponse<WorkoutDto>> GetPagedAsync(Guid accountId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Workouts
                .AsNoTracking()
                .Where(w => w.AccountId == accountId);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(w => w.WorkoutSets)
                .OrderByDescending(w => w.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedListResponse<WorkoutDto>
            {
                Items = _mapper.ToDtoList(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<List<DateOnly>> GetActivityAsync(Guid accountId, CancellationToken cancellationToken)
        {
            var dates = await _context.Workouts
                .AsNoTracking()
                .Where(w => w.AccountId == accountId)
                .Select(w => w.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync(cancellationToken);

            return dates.Select(DateOnly.FromDateTime).ToList();
        }

        private static List<WorkoutSet> MapSets(List<WorkoutSetDto> sets)
        {
            return sets.Select(s => new WorkoutSet
            {
                ExerciseId = s.ExerciseId,
                SetNumber = s.SetNumber,
                Reps = s.Reps,
                Weight = s.Weight
            }).ToList();
        }
    }
}
