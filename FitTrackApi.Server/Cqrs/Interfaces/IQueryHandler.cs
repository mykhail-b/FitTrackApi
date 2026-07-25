namespace FitTrackApi.Server.Cqrs.Interfaces;

public interface IQueryHandler <in TQuery, TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
}

public interface IQueryHandler<TResult>
{
    Task<TResult> Handle(CancellationToken cancellationToken = default);
}