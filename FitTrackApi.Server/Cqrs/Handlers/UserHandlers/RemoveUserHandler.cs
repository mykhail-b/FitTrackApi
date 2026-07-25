using FitTrackApi.Server.Cqrs.Interfaces;
using FitTrackApi.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace FitTrackApi.Server.Cqrs.Handlers.UserHandlers;

public record RemoveUserCommand(string UserId);

public class RemoveUserHandler : ICommandHandler<RemoveUserCommand, bool>
{
    private readonly DataContext _dbContext;

    public RemoveUserHandler(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(RemoveUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user is null)
            return false;

        _dbContext.Users.Remove(user);
        var affected = await _dbContext.SaveChangesAsync(cancellationToken);

        return affected > 0;
    }
}
