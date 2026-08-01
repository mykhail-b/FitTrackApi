using FitTrackApi.Application.Dto.User;
using FitTrackApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Infrastructure.Services;

public interface IUserService
{
    Task<UserInfoDto?> GetUserInfoAsync(string userId, CancellationToken ct = default);
    Task<bool> UpdateUserInfoAsync(string userId, UserInfoDto dto, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default);
}

public class UserService : IUserService
{
    private readonly DataContext _dbContext;
    public UserService(DataContext dbContext) => _dbContext = dbContext;

    public async Task<UserInfoDto?> GetUserInfoAsync(string userId, CancellationToken ct = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoDto { FullName = u.FullName, Email = u.Email })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> UpdateUserInfoAsync(string userId, UserInfoDto dto, CancellationToken ct = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user is null) return false;

        user.FullName = dto.FullName!;
        user.Email = dto.Email;

        var affected = await _dbContext.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user is null) return false;

        _dbContext.Users.Remove(user);
        var affected = await _dbContext.SaveChangesAsync(ct);
        return affected > 0;
    }
}