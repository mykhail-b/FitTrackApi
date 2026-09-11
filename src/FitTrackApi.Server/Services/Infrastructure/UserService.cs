using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Server.Dto.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Services.Infrastructure;

public interface IUserService
{
    Task<AccountDto> GetUserInfoAsync(string userId, CancellationToken ct = default);
    Task<AccountDto> UpdateUserInfoAsync(string userId, AccountDto dto, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default);
}

public class UserService : IUserService
{
    private readonly DataContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(DataContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<AccountDto> GetUserInfoAsync(string userId, CancellationToken ct = default)
    {
        var account = await _dbContext.Accounts
            .Include(a => a.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.UserId == userId, ct);

        if (account is null)
            throw new KeyNotFoundException($"Account '{userId}' not found.");

        return new AccountDto(
            account.FullName,
            account.User.Email!,
            account.BirthDate,
            account.Gender
        );
    }

    public async Task<AccountDto> UpdateUserInfoAsync(string userId, AccountDto dto, CancellationToken ct = default)
    {
        var account = await _dbContext.Accounts
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.UserId == userId, ct);

        if (account is null)
            throw new KeyNotFoundException($"Account '{userId}' not found.");

        // Update Account fields
        account.FullName = dto.FullName;
        account.BirthDate = dto.BirthDate;
        account.Gender = dto.Gender;

        // Update Identity email
        if (account.User.Email != dto.Email)
        {
            var result = await _userManager.SetEmailAsync(account.User, dto.Email);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        await _dbContext.SaveChangesAsync(ct);

        return dto;
    }

    public async Task<bool> DeleteUserAsync(string userId, CancellationToken ct = default)
    {
        var account = await _dbContext.Accounts
            .Include(a => a.User)
            .Include(a => a.Meals)
            .Include(a => a.Workouts)
            .FirstOrDefaultAsync(a => a.UserId == userId, ct);

        if (account is null)
            return false;

        // Remove domain data
        _dbContext.Meals.RemoveRange(account.Meals);
        _dbContext.Workouts.RemoveRange(account.Workouts);
        _dbContext.Accounts.Remove(account);

        // Remove Identity user
        await _userManager.DeleteAsync(account.User);

        await _dbContext.SaveChangesAsync(ct);

        return true;
    }
}