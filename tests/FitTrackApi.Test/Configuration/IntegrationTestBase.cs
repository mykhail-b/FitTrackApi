using FitTrackApi.Application.Interfaces.RepositoryDI;
using FitTrackApi.Infrastructure.Data;
using FitTrackApi.Infrastructure.Repository;

namespace FitTrackApi.Test.Configuration;

[Collection(DatabaseCollection.Name)]
public abstract class IntegrationTestBase(DatabaseFixture fixture) : IAsyncLifetime
{
    protected DatabaseFixture Fixture { get; } = fixture;

    protected DataContext DbContext { get; private set; } = null!;
    protected IUnitOfWork UnitOfWork { get; private set; } = null!;

    public async ValueTask InitializeAsync()
    {
        await Fixture.ResetDatabaseAsync();

        DbContext = Fixture.CreateContext();
        UnitOfWork = new UnitOfWork(DbContext);
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }
}