using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using FitTrackApi.Core.Dto.User;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.UserHandlers;

public record GetUserInfoQuery(string userId);
public class GetUserInfoHandler : IQueryHandler<GetUserInfoQuery, UserInfoDto>
{
    private readonly DataContext _dbContext;

    public GetUserInfoHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserInfoDto?> Handle(GetUserInfoQuery query, CancellationToken cancellationToken = default)
    {
        // Возвращаем null, если пользователь не найден, вместо throw —
        // контроллер сам решает, что вернуть (404), а не падает с 500.
        return await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Id == query.userId)
            .Select(u => new UserInfoDto
            {
                FullName = u.FullName,
                Email = u.Email
            })
            .FirstOrDefaultAsync();
    }
}

public record UpdateUserInfoCommand(string UserId, UserInfoDto UpdatedUserInfo);

public class UpdateUserInfoHandler : ICommandHandler<UpdateUserInfoCommand, bool>
{
    private readonly DataContext _dbContext;

    public UpdateUserInfoHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateUserInfoCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user is null)
            return false; // контроллер вернёт NotFound() по false, без лишнего throw

        user.FullName = command.UpdatedUserInfo.FullName!;
        user.Email = command.UpdatedUserInfo.Email;

        var affected = await _dbContext.SaveChangesAsync(cancellationToken);

        return affected > 0;
    }
}