using FitTrackApi.Application.Dto.User;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Services;

public interface IUserService
{
    Task<UserInfoDto> GetUserInfoAsync(string userId, CancellationToken ct = default);
    Task<UserInfoDto> UpdateUserInfoAsync(string userId, UserInfoDto dto, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default);
}

public class UserService : IUserService
{
    private readonly DataContext _dbContext;
    public UserService(DataContext dbContext) => _dbContext = dbContext;

    public async Task<UserInfoDto> GetUserInfoAsync(string userId, CancellationToken ct = default)
    {
        var user =  await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoDto { FullName = u.FullName, Email = u.Email })
            .FirstOrDefaultAsync(ct);

        if (user is null)
            throw new KeyNotFoundException($"User '{userId}' not found.");

        return user;
    }

    public async Task<UserInfoDto> UpdateUserInfoAsync(string userId, UserInfoDto dto, CancellationToken ct = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user is null)
            throw new KeyNotFoundException($"User '{userId}' not found.");

        user.FullName = dto.FullName!;
        user.Email = dto.Email;

        await _dbContext.SaveChangesAsync(ct);

        return new UserInfoDto { FullName = user.FullName, Email = user.Email }; 
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