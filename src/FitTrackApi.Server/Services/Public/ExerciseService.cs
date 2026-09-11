using FitTrackApi.Application.Dto;
using FitTrackApi.Application.Dto.Exercise;
using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Server.Domain.Entity;
using FitTrackApi.Server.Mappers;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Services.Public
{
    public interface IExerciseService
    {
        Task<ExerciseResponse> CreateAsync(CreateExerciseRequest request, CancellationToken cancellationToken);
        Task<ExerciseResponse> UpdateAsync(Guid id, UpdateExerciseRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<ExerciseResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PagedListResponse<ExerciseShortResponse>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    }
    public class ExerciseService : IExerciseService
    {
        private readonly DataContext _context;
        private readonly IExerciseMapper _mapper;

        public ExerciseService(DataContext context, IExerciseMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExerciseResponse> CreateAsync(CreateExerciseRequest request, CancellationToken cancellationToken)
        {
            var exercise = new Exercise
            {
                Name = request.Name,
                Force = request.Force,
                Mechanic = request.Mechanic,
                Equipment = request.Equipment,
                Category = request.Category,
                Muscle = request.Muscle,
                Instruction = request.Instruction,
                Images = request.Images,
            };

            await _context.Exercises.AddAsync(exercise, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.ToResponse(exercise);
        }

        public async Task<ExerciseResponse> UpdateAsync(Guid id, UpdateExerciseRequest request, CancellationToken cancellationToken)
        {
            var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (exercise == null)
                throw new KeyNotFoundException($"Exercise {id} not found");

            exercise.Name = request.Name;
            exercise.Force = request.Force;
            exercise.Mechanic = request.Mechanic;
            exercise.Equipment = request.Equipment;
            exercise.Category = request.Category;
            exercise.Muscle = request.Muscle;
            exercise.Instruction = request.Instruction;
            exercise.Images = request.Images;

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.ToResponse(exercise);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (exercise == null)
                throw new KeyNotFoundException($"Exercise with id {id} not found");

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<ExerciseResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var exercise = await _context.Exercises.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (exercise == null)
                throw new KeyNotFoundException($"Exercise {id} not found");

            return _mapper.ToResponse(exercise);
        }

        public async Task<PagedListResponse<ExerciseShortResponse>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Exercises.AsNoTracking();

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(e => e.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedListResponse<ExerciseShortResponse>
            {
                Items = _mapper.ToShortResponseList(items),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
