using FitTrackApi.Application.Dto.User;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Services;

public interface IUserService
{
    Task<UserDto> GetUserInfoAsync(string userId, CancellationToken ct = default);
    Task<UserDto> UpdateUserInfoAsync(string userId, UserDto dto, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default);
}

public class UserService : IUserService
{
    private readonly DataContext _dbContext;
    public UserService(DataContext dbContext) => _dbContext = dbContext;

    public async Task<UserDto> GetUserInfoAsync(string userId, CancellationToken ct = default)
    {
        var user =  await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserDto ( u.FullName, u.Email, u.BirthDate, u.Gender ))
            .FirstOrDefaultAsync(ct);

        if (user is null)
            throw new KeyNotFoundException($"User '{userId}' not found.");

        return user;
    }

    public async Task<UserDto> UpdateUserInfoAsync(string userId, UserDto dto, CancellationToken ct = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user is null)
            throw new KeyNotFoundException($"User '{userId}' not found.");

        user.FullName = dto.FullName!;
        user.Email = dto.Email;

        await _dbContext.SaveChangesAsync(ct);

        return new UserDto ( user.FullName, user.Email, user.BirthDate, user.Gender ); 
    }

    public async Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user is null) return false;

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(ct);

        return true;
    }
}